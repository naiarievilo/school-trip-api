using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Infrastructure.WebScraping.Abstractions;

public interface IBrowserService<TBrowser, TPage> : IAsyncDisposable
{
    Task<Result<TBrowser>> GetBrowserAsync();
    Task<Result<TPage>> GetPageAsync();
    Task<Result> ReleasePageAsync(TPage page);
}