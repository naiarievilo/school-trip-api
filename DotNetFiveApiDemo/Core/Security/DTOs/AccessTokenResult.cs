using System;

namespace DotNetFiveApiDemo.Core.Security.DTOs
{
    public class AccessTokenResult
    {
        public string AccessToken { get; init; }
        public DateTime ExpiresAt { get; init; }
    }
}