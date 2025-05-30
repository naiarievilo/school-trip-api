namespace DotNetFiveApiDemo.Core.Security.DTOs
{
    public sealed class AuthenticationTokensResult : AccessTokenResult
    {
        public string RefreshToken { get; init; }
    }
}