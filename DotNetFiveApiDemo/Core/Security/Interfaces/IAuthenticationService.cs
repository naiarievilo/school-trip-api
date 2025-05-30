using System.Threading.Tasks;
using DotNetFiveApiDemo.Core.Common.DTOs;
using DotNetFiveApiDemo.Core.Common.DTOs.Base;
using DotNetFiveApiDemo.Core.Common.Interfaces;
using DotNetFiveApiDemo.Core.Security.DTOs;
using DotNetFiveApiDemo.WebApi.User.DTOs;

namespace DotNetFiveApiDemo.Core.Security.Interfaces
{
    public interface IAuthenticationService<TUser> where TUser : class, IAggregateRoot
    {
        Task<Result<AuthenticationTokensResult>> IssueAuthenticationTokensAsync(TUser user);
        Task<Result<AuthenticationTokensResult>> IssueAuthenticationTokensAsync(RefreshAccessTokenRequest request);
        Task<Result> SendEmailConfirmationLinkAsync(TUser user, string emailConfirmationToken);
        Task<Result> SendPasswordResetCodeAsync(TUser user, string resetCode);
        Task<Result> SendUnlockUserEmailAsync(string email, string passwordResetCode);
    }
}