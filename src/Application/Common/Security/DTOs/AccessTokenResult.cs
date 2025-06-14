namespace SchoolTripApi.Application.Common.Security.DTOs;

public class AccessTokenResult
{
    protected AccessTokenResult(string accessToken, DateTime expiresAt)
    {
        AccessToken = accessToken;
        ExpiresAt = expiresAt;
    }

    public string AccessToken { get; private set; }
    public DateTime ExpiresAt { get; private set; }

    public static AccessTokenResult From(string accessToken, DateTime expiresAt)
    {
        return new AccessTokenResult(accessToken, expiresAt);
    }
}