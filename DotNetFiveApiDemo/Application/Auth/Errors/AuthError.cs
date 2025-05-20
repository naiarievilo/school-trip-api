using DotNetFiveApiDemo.Application.Common.DTOs;

namespace DotNetFiveApiDemo.Application.Auth.Errors
{
    public class AuthError : Error
    {
        public AuthError(string code, string description) : base(code, description)
        {
        }

        public static string AccessTokenNotExpiredCode => "AuthError.AccessTokenExpired";
        public static string RefreshTokenExpiredCode => "AuthError.RefreshTokenExpired";

        public static Error AccessTokenNotExpired =>
            new AuthError(AccessTokenNotExpiredCode, "Access token must be expired.");

        public static Error RefreshTokenExpired =>
            new AuthError(RefreshTokenExpiredCode, "Refresh token is expired. Please log in again.");
    }
}