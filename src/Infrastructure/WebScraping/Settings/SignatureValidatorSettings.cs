namespace SchoolTripApi.Infrastructure.WebScraping.Settings;

public class SignatureValidatorSettings
{
    public required string ValidatorUrl { get; init; }
    public required int ValidationTimeout { get; init; }
}