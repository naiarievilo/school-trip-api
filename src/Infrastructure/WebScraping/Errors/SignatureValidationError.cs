using SchoolTripApi.Domain.Common.Errors;

namespace SchoolTripApi.Infrastructure.WebScraping.Errors;

public class SignatureValidationError(string code, string description) : Error(code, description)
{
    private const string ValidationFailedCode = "SignatureValidationError.ValidationFailed";

    private const string ExtractFileValidationInfoFailedCode =
        "SignatureValidationError.ExtractFileValidationInfoFailed";

    private const string ExtractSignatureInfoFailedCode = "SignatureValidationError.ExtractSignatureInfoFailed";

    private const string ExtractValidationReportFailedCode = "SignatureValidationError.ExtractValidationReportFailed";

    public static Error ValidationFailed(string errorMessage)
    {
        return new SignatureValidationError(ValidationFailedCode, errorMessage);
    }

    public static Error ExtractFileValidationInfoFailed(string errorMessage)
    {
        return new SignatureValidationError(ExtractFileValidationInfoFailedCode, errorMessage);
    }

    public static Error ExtractSignatureInfoFailed(string errorMessage)
    {
        return new SignatureValidationError(ExtractSignatureInfoFailedCode, errorMessage);
    }

    public static Error ExtractValidationReportFailed(string errorMessage)
    {
        return new SignatureValidationError(ExtractValidationReportFailedCode, errorMessage);
    }
}