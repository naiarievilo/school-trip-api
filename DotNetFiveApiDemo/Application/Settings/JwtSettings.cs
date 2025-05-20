using System.Collections.Generic;

namespace DotNetFiveApiDemo.Application.Settings
{
    public class JwtSettings
    {
        public string Secret { get; init; }
        public string Issuer { get; init; }
        public string Audience { get; init; }
        public int ExpirationInMinutes { get; init; }
        public Dictionary<string, TokenTypesSettings> TokenTypes { get; set; } = new();
    }
}