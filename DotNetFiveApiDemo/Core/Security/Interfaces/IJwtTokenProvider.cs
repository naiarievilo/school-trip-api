using System.Threading.Tasks;
using DotNetFiveApiDemo.Core.Common.DTOs;
using DotNetFiveApiDemo.Core.Security.DTOs;

namespace DotNetFiveApiDemo.Core.Security.Interfaces
{
    public interface IJwtTokenProvider
    {
        AccessTokenResult IssueAccessToken(int userId);
        Task<Result<AuthenticationTokensResult>> RefreshAccessTokenAsync(string refreshToken);
        Task<Result<string>> IssueRefreshTokenAsync(int userId, string tokenFamily = null);
    }
}