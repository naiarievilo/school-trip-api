namespace SchoolTripApi.Application.Common.Security.DTOs;

public sealed class AuthenticationTokensResult : AccessTokenResult
{
    private AuthenticationTokensResult(string accessToken, DateTime expiresAt, string refreshToken) : base(accessToken,
        expiresAt)
    {
        RefreshToken = refreshToken;
    }

    public string RefreshToken { get; private set; }

    public static AuthenticationTokensResult From(string accessToken, DateTime expiresAt, string refreshToken)
    {
        return new AuthenticationTokensResult(accessToken, expiresAt, refreshToken);
    }
}