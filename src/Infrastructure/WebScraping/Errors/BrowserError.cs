using SchoolTripApi.Domain.Common.Errors;

namespace SchoolTripApi.Infrastructure.WebScraping.Errors;

public sealed class BrowserError(string code, string description) : Error(code, description)
{
    private const string FailedToReleasePageCode = "BrowserError.FailedToReleasePage";
    private const string FailedToInitializeBrowserCode = "BrowserError.FailedToInitializeBrowser";

    public static Error FailedToReleasePage()
    {
        return new BrowserError(FailedToReleasePageCode, "Failed to dispose page properly.");
    }

    public static Error FailedToInitializeBrowser(string message)
    {
        return new BrowserError(FailedToInitializeBrowserCode, message);
    }
}