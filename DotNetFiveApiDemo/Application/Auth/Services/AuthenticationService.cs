using System.Collections.Generic;
using DotNetFiveApiDemo.Application.Auth.Errors;
using DotNetFiveApiDemo.Application.Auth.Interfaces;
using DotNetFiveApiDemo.Application.Common.DTOs;
using DotNetFiveApiDemo.Application.Common.DTOs.Base;
using DotNetFiveApiDemo.Application.Settings;
using DotNetFiveApiDemo.Application.User.Identity;
using DotNetFiveApiDemo.WebApi.DTOs;

namespace DotNetFiveApiDemo.Application.Auth.Services
{
    public class AuthenticationService : IAuthenticationService<ApplicationUser>
    {
        public static readonly string AccessTokenKey = "accessToken";
        public static readonly string RefreshTokenKey = "refreshToken";
        private readonly IJwtTokenProvider<ApplicationUser> _jwtTokenProvider;

        public AuthenticationService(IJwtTokenProvider<ApplicationUser> jwtTokenProvider)
        {
            _jwtTokenProvider = jwtTokenProvider;
        }

        public Dictionary<string, string> GenerateAccessAndRefreshTokens(ApplicationUser user)
        {
            var accessToken = _jwtTokenProvider.GenerateToken(user, JwtTokenTypes.AccessToken);
            var refreshToken = _jwtTokenProvider.GenerateToken(user, JwtTokenTypes.RefreshToken);
            return new Dictionary<string, string>
            {
                { AccessTokenKey, accessToken },
                { RefreshTokenKey, refreshToken }
            };
        }

        public bool ValidateToken(string token, JwtTokenTypes type)
        {
            return _jwtTokenProvider.IsTokenValid(token, type);
        }

        public Result<Dictionary<string, string>> RefreshAccessToken(RefreshTokenCommand command)
        {
            var accessToken = command.AccessToken;
            var refreshToken = command.RefreshToken;

            var isAccessTokenValid = _jwtTokenProvider.IsTokenValid(accessToken, JwtTokenTypes.AccessToken);
            if (isAccessTokenValid) return Result.Failure<Dictionary<string, string>>(AuthError.AccessTokenNotExpired);

            var isRefreshTokenValid = _jwtTokenProvider.IsTokenValid(refreshToken, JwtTokenTypes.RefreshToken);
            if (!isRefreshTokenValid) return Result.Failure<Dictionary<string, string>>(AuthError.RefreshTokenExpired);

            var newAccessToken = _jwtTokenProvider.GenerateToken(refreshToken, JwtTokenTypes.AccessToken);
            return Result.Success(new Dictionary<string, string>
            {
                { AccessTokenKey, newAccessToken }
            });
        }
    }
}