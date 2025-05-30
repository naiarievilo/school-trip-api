using System;

namespace DotNetFiveApiDemo.Application.User.DTOs
{
    public class SignInUserResult
    {
        public int UserId { get; init; }
        public bool IsEmailConfirmed { get; init; }
        public string AccessToken { get; init; }
        public DateTime ExpiresAt { get; init; }
        public string RefreshToken { get; init; }
    }
}