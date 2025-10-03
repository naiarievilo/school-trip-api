using System.Collections.Concurrent;
using Microsoft.Extensions.Options;
using PuppeteerSharp;
using SchoolTripApi.Application.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Infrastructure.WebScraping.Abstractions;
using SchoolTripApi.Infrastructure.WebScraping.Errors;
using SchoolTripApi.Infrastructure.WebScraping.Settings;

namespace SchoolTripApi.Infrastructure.WebScraping.Services;

public class BrowserService : IBrowserService<IBrowser, IPage>
{
    private readonly BrowserSettings _browserSettings;
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private readonly IAppLogger<BrowserService> _logger;
    private readonly int _maxPagePoolSize;
    private readonly Timer _pageCleanupTimer;
    private readonly ConcurrentDictionary<Guid, PageWrapper> _pagePool = [];
    private IBrowser? _browser;
    private bool _isInitialized;

    public BrowserService(IAppLogger<BrowserService> logger, IOptions<BrowserSettings> browserOptions)
    {
        _logger = logger;
        _browserSettings = browserOptions.Value;
        _maxPagePoolSize = _browserSettings.MaxPagePoolSize;

        var pageCleanupTimeSpan = TimeSpan.FromMinutes(_browserSettings.PageCleanUpTimer);
        _pageCleanupTimer = new Timer(CleanupStalePages, null, pageCleanupTimeSpan, pageCleanupTimeSpan);
    }

    public async ValueTask DisposeAsync()
    {
        _logger.LogInformation("Disposing browser service...");

        while (_pagePool.TryTake(out var page))
            try
            {
                await page.CloseAsync();
                await page.DisposeAsync();
            }
            catch (Exception ex)
            {
                var pageName = await page.GetTitleAsync();
                _logger.LogError(ex, "Failed to close and dispose of page {pageName}: {errorMessage}", pageName,
                    ex.Message);
            }

        if (_browser is not null)
            try
            {
                await _browser.CloseAsync();
                await _browser.DisposeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to close and dispose of browser service: {errorMessage}", ex.Message);
            }

        _initLock.Dispose();
    }

    public async Task<Result> ReleasePageAsync(IPage? page)
    {
        if (page is null) return Result.Success();

        try
        {
            await page.EvaluateExpressionAsync("window.localStorage.clear()");
            await page.EvaluateExpressionAsync("window.sessionStorage.clear()");

            var pageWrapperFound = false;
            foreach (var pageWrapper in _pagePool.Values)
                if (pageWrapper.Page == page)
                {
                    pageWrapper.LastUsed = DateTime.UtcNow;
                    Interlocked.Exchange(ref pageWrapper.InUse, 0);
                    pageWrapperFound = true;
                    break;
                }

            if (pageWrapperFound) await page.CloseAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to release page: {errorMessage}. Page disposed instead.", ex.Message);
            await page.DisposeAsync();
            return Result.Failure(BrowserError.FailedToReleasePage);
        }
    }

    public async Task<Result<IBrowser>> GetBrowserAsync()
    {
        if (_isInitialized && _browser is not null) return Result.Success(_browser);

        try
        {
            _logger.LogInformation("Initializing browser...");

            await _initLock.WaitAsync();

            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            var browserLaunchOptions = BuildBrowserLaunchOptions();
            _browser = await Puppeteer.LaunchAsync(browserLaunchOptions);
            _isInitialized = true;

            _logger.LogInformation("Browser initialized successfully.");
            return Result.Success(_browser);
        }
        catch (Exception ex)
        {
            var errorMessage = $"Failed to initialize browser: {ex.Message}";
            _logger.LogError(ex, errorMessage);
            return Result.Failure<IBrowser>(BrowserError.FailedToInitializeBrowser(errorMessage));
        }
        finally
        {
            _initLock.Release();
        }
    }

    public async Task<Result<IPage>> GetPageAsync()
    {
        while (Date)
            foreach (var kvp in _pagePool)
            {
                var pageWrapper = kvp.Value;
                if (Interlocked.CompareExchange(ref pageWrapper.InUse, 1, 0) != 0) continue;

                try
                {
                    await pageWrapper.Page.EvaluateExpressionAsync("1+1");
                    pageWrapper.LastUsed = DateTime.UtcNow;
                    return Result.Success(pageWrapper.Page);
                }
                catch
                {
                    _pagePool.TryRemove(pageWrapper.Id, out _);
                    try
                    {
                        await pageWrapper.Page.DisposeAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to dispose invalid page property: {errorMessage}", ex.Message);
                    }
                }
            }

        var getBrowser = await GetBrowserAsync();
        if (getBrowser.Failed) return Result.Failure<IPage>(getBrowser.Error);
        var browser = getBrowser.Value;

        var newPage = await browser.NewPageAsync();

        await ConfigurePageAsync(newPage);

        if (_pagePool.Count >= _maxPagePoolSize) return Result.Success(newPage);

        var newPageWrapper = PageWrapper.Create(newPage);
        _pagePool.TryAdd(newPageWrapper.Id, newPageWrapper);
        return Result.Success(newPage);
    }

    private static async Task ConfigurePageAsync(IPage newPage)
    {
        await newPage.SetCacheEnabledAsync();
        await newPage.SetRequestInterceptionAsync(true);

        // Block unnecessary resources
        newPage.Request += (sender, e) =>
        {
            var request = e.Request;
            _ = request.ResourceType is ResourceType.Image or ResourceType.StyleSheet or ResourceType.Font
                or ResourceType.Media
                ? request.AbortAsync()
                : request.ContinueAsync();
        };
    }

    private LaunchOptions BuildBrowserLaunchOptions()
    {
        return new LaunchOptions
        {
            Headless = true,
            Args = _browserSettings.LaunchArgs.Split(_browserSettings.LaunchArgsSeparator),
            DefaultViewport = new ViewPortOptions
            {
                Width = _browserSettings.ViewPort.Width,
                Height = _browserSettings.ViewPort.Height
            },
            Timeout = _browserSettings.BrowserInitializationTimeout
        };
    }

    private async void CleanupStalePages(object? obj)
    {
        var staleThreshold = DateTime.UtcNow.AddMinutes(-_browserSettings.PageExpiresIn);
        var wrappersToRemove = new List<PageWrapper>();

        foreach (var wrapper in _pagePool)
            if (wrapper.LastUsed < staleThreshold && Interlocked.CompareExchange(ref wrapper.InUse, 1, 0) == 0)
                wrappersToRemove.Add(wrapper);

        foreach (var wrapper in wrappersToRemove) _pagePool.TryTake(out _);
    }

    internal sealed class PageWrapper
    {
        public int InUse;

        private PageWrapper(IPage page)
        {
            Id = Guid.NewGuid();
            Page = page;
            InUse = 1;
            LastUsed = DateTime.UtcNow;
        }

        public Guid Id { get; }
        public IPage Page { get; }
        public DateTime LastUsed { get; set; }

        public static PageWrapper Create(IPage page)
        {
            return new PageWrapper(page);
        }
    }
}