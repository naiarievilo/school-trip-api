namespace SchoolTripApi.Infrastructure.WebScraping.Settings;

public class BrowserSettings
{
    public required string LaunchArgs { get; init; }
    public required char LaunchArgsSeparator { get; init; }
    public required DefaultViewport ViewPort { get; init; }
    public required int MaxPagePoolSize { get; init; }
    public required int BrowserInitializationTimeout { get; init; }
    public required int GetPageTimeout { get; init; }
    public required int PageCleanUpTimer { get; init; }
    public required int PageExpiresIn { get; init; }
}