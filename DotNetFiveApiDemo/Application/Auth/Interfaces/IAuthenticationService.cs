using System.Collections.Generic;
using DotNetFiveApiDemo.Application.Common.DTOs;
using DotNetFiveApiDemo.Application.Settings;
using DotNetFiveApiDemo.WebApi.DTOs;

namespace DotNetFiveApiDemo.Application.Auth.Interfaces
{
    public interface IAuthenticationService<TUser> where TUser : class
    {
        public Dictionary<string, string> GenerateAccessAndRefreshTokens(TUser user);
        public bool ValidateToken(string token, JwtTokenTypes type);
        public Result<Dictionary<string, string>> RefreshAccessToken(RefreshTokenCommand command);
        
    }
}