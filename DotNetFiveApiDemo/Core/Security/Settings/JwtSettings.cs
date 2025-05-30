namespace DotNetFiveApiDemo.Core.Security.Settings
{
    internal class JwtSettings
    {
        public string Secret { get; init; }
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public int AccessTokenExpiresAtInMinutes { get; init; }
        public int RefreshTokenExpiresAtInDays { get; init; }
    }
}