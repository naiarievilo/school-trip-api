using DotNetFiveApiDemo.Application.Settings;

namespace DotNetFiveApiDemo.Application.Auth.Interfaces
{
    public interface IJwtTokenProvider<TUser> where TUser : class
    {
        string GenerateToken(TUser user, JwtTokenTypes tokenType);
        string GenerateToken(string refreshToken, JwtTokenTypes typeSettings);
        bool IsTokenValid(string token, JwtTokenTypes tokenType);
    }
}