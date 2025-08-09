using SchoolTripApi.Domain.Common.Errors;

namespace SchoolTripApi.Application.Common.Security.Errors;

public sealed class SecurityError : Error
{
    private const string FailedToIssueRefreshTokenCode = "SecurityError.FailedToIssueNewRefreshToken";
    private const string RefreshTokenExpiredCode = "SecurityError.RefreshTokenExpired";
    private const string ClientUrlNotProvidedCode = "SecurityError.ClientUrlNotProvided";

    private SecurityError(string code, string description) : base(code, description)
    {
    }

    public static Error RefreshTokenExpired =>
        new SecurityError(RefreshTokenExpiredCode, "Refresh token is expired. Please log in again.");

    public static Error ClientUrlNotProvided =>
        new SecurityError(ClientUrlNotProvidedCode, "Client callback URL not set.");

    public static Error FailedToIssueNewRefreshToken =>
        new SecurityError(FailedToIssueRefreshTokenCode, "Failed to issue refresh token. Try again.");
}