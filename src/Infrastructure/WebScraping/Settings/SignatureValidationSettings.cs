namespace SchoolTripApi.Infrastructure.WebScraping.Settings;

public class SignatureValidationSettings
{
    public required string ValidationUrl { get; init; }
    public required int ValidationTimeout { get; init; }
}