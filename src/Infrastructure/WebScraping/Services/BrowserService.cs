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
    private readonly ConcurrentDictionary<Guid, PageWrapper> _allPages = [];
    private readonly BrowserSettings _browserSettings;
    private readonly SemaphoreSlim _initLock = new(1, 1);
    private readonly IAppLogger<BrowserService> _logger;
    private readonly int _maxPagePoolSize;
    private readonly Timer _pageCleanupTimer;
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

    public async Task<Result> ReleasePageAsync(IPage? page)
    {
        if (page is null) return Result.Success();

        var pageWrapper = _allPages.Values.FirstOrDefault(pageWrapper => pageWrapper.Page == page);
        if (pageWrapper is null)
        {
            _logger.LogWarning("Attempted to release an unknown page; disposing it instead...");
            try
            {
                await page.CloseAsync();
                await page.DisposeAsync();
                return Result.Success();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to dispose unknown page: {exMessage}", ex.Message);
                return Result.Failure(BrowserError.FailedToReleasePage);
            }
        }

        if (pageWrapper.IsPooled)
            try
            {
                // Clean up page state for reuse
                await page.EvaluateExpressionAsync("window.localStorage.clear()");
                await page.EvaluateExpressionAsync("windows.sessionStorage.clear()");

                // Navigate to about:blank to free memory
                await page.GoToAsync("about:blank");

                pageWrapper.LastUsed = DateTime.UtcNow;
                Interlocked.Exchange(ref pageWrapper.InUse, 0);

                _logger.LogDebug("Released pooled page '{pageId}' back to pool", pageWrapper.Id);
                return Result.Success();
            }
            catch (Exception releaseEx)
            {
                _logger.LogError(releaseEx,
                    "Error releasing pooled page '{pageId}': {errorMessage}. Removing it from pool...", pageWrapper.Id,
                    releaseEx);
                _allPages.TryRemove(pageWrapper.Id, out _);
                try
                {
                    await page.DisposeAsync();
                    return Result.Success();
                }
                catch (Exception disposeEx)
                {
                    _logger.LogError(disposeEx,
                        "Error disposing failed-to-release pooled page '{pageId}': {errorMessage}", pageWrapper.Id,
                        disposeEx);
                    return Result.Failure(BrowserError.FailedToReleasePage);
                }
            }

        _logger.LogDebug("Disposing non-pooled page '{pageId}'", pageWrapper.Id);

        _allPages.TryRemove(pageWrapper.Id, out _);
        try
        {
            await pageWrapper.Page.CloseAsync();
            await pageWrapper.Page.DisposeAsync();
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to release non-pooled page '{pageId}': {errorMessage}", pageWrapper.Id,
                ex.Message);
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
        int pooledCount;
        var startTime = DateTime.UtcNow;

        while ((DateTime.UtcNow - startTime).TotalMilliseconds < _browserSettings.GetPageTimeout)
        {
            foreach (var pageWrapper in _allPages.Values)
            {
                if (!pageWrapper.IsPooled) continue;
                if (Interlocked.CompareExchange(ref pageWrapper.InUse, 1, 0) != 0) continue;

                try
                {
                    await pageWrapper.Page.EvaluateExpressionAsync("1+1");
                    pageWrapper.LastUsed = DateTime.UtcNow;
                    _logger.LogDebug("Reusing pooled page '{pageId}'.", pageWrapper.Id);
                    return Result.Success(pageWrapper.Page);
                }
                catch
                {
                    _logger.LogWarning("Pooled page '{pageId}' is invalid; disposing it instead...",
                        pageWrapper.Id);
                    _allPages.TryRemove(pageWrapper.Id, out _);
                    try
                    {
                        await pageWrapper.Page.DisposeAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to dispose invalid pooled page '{pageId}': {errorMessage}",
                            ex.Message);
                    }
                }
            }

            pooledCount = _allPages.Values.Count(pageWrapper => pageWrapper.IsPooled);
            if (pooledCount >= _maxPagePoolSize)
            {
                await Task.Delay(50);
                continue;
            }

            break;
        }

        var getBrowser = await GetBrowserAsync();
        if (getBrowser.Failed) return Result.Failure<IPage>(getBrowser.Error);
        var browser = getBrowser.Value;

        var newPage = await browser.NewPageAsync();
        await ConfigurePageAsync(newPage);

        pooledCount = _allPages.Values.Count(pageWrapper => pageWrapper.IsPooled);
        var shouldPool = pooledCount < _maxPagePoolSize;

        var newPageWrapper = PageWrapper.Create(newPage, shouldPool);
        if (!_allPages.TryAdd(newPageWrapper.Id, newPageWrapper))
            _logger.LogError("Failed to track new page '{pageId}'.", newPageWrapper.Id);

        _logger.LogDebug("Created new {PageType} page '{pageId}'", shouldPool ? "pooled" : "non-pooled",
            newPageWrapper.Id);
        return Result.Success(newPageWrapper.Page);
    }

    public async ValueTask DisposeAsync()
    {
        _logger.LogInformation("Disposing browser service...");

        await _pageCleanupTimer.DisposeAsync();

        foreach (var pageWrapper in _allPages.Values)
            try
            {
                await pageWrapper.Page.CloseAsync();
                await pageWrapper.Page.DisposeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error disposing page '{pageId}': {errorMessage}", pageWrapper.Id, ex.Message);
            }

        _allPages.Clear();

        if (_browser is not null)
            try
            {
                await _browser.CloseAsync();
                await _browser.DisposeAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError("Error disposing browser: {errorMessage}", ex.Message);
            }

        _initLock.Dispose();
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
        try
        {
            var staleThreshold = DateTime.UtcNow.AddMinutes(-_browserSettings.PageExpiresIn);
            var pagesToRemove = (from kvp in _allPages
                let pageWrapper = kvp.Value
                where pageWrapper.IsPooled
                where pageWrapper.LastUsed < staleThreshold &&
                      Interlocked.CompareExchange(ref pageWrapper.InUse, 1, 0) == 0
                select kvp).ToList();

            foreach (var kvp in pagesToRemove)
                if (_allPages.TryRemove(kvp.Key, out var pageWrapper))
                    try
                    {
                        await pageWrapper.Page.CloseAsync();
                        await pageWrapper.Page.DisposeAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to dispose stale, pooled page '{pageId}': {errorMessage}", kvp.Key,
                            ex.Message);
                    }

            _logger.LogInformation("Cleaned up {Count} stale pages from pool.", pagesToRemove.Count);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Stale page cleanup failed: {errorMessage}", e.Message);
        }
    }


    internal sealed class PageWrapper
    {
        public int InUse;

        private PageWrapper(IPage page, bool isPooled)
        {
            Id = Guid.NewGuid();
            Page = page;
            InUse = 1;
            LastUsed = DateTime.UtcNow;
            IsPooled = isPooled;
        }

        public Guid Id { get; }
        public IPage Page { get; }
        public DateTime LastUsed { get; set; }
        public bool IsPooled { get; set; }

        public static PageWrapper Create(IPage page, bool isPooled)
        {
            return new PageWrapper(page, isPooled);
        }
    }
}