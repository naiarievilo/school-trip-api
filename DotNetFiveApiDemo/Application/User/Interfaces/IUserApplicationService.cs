using System.Threading.Tasks;
using DotNetFiveApiDemo.Application.User.DTOs;
using DotNetFiveApiDemo.Core.Common.DTOs;
using DotNetFiveApiDemo.Core.Common.DTOs.Base;
using DotNetFiveApiDemo.Core.Security.DTOs;
using DotNetFiveApiDemo.Core.User.DTOs;
using DotNetFiveApiDemo.WebApi.User.DTOs;

namespace DotNetFiveApiDemo.Application.User.Interfaces
{
    public interface IUserApplicationService
    {
        Task<Result<SignUpUserResult>> SignUpUserAsync(SignUpUserRequest request);
        Task<Result<UserDto>> GetUserAsync(int userId);
        Task<Result<SignInUserResult>> SignInUserAsync(SignInUserRequest request);
        Task<Result> UpdateUserInfoAsync(UpdateUserInfoRequest request, int userId);
        Task<Result<UpdateUserEmailResult>> UpdateUserEmailAsync(UpdateUserEmailRequest request, int userId);
        Task<Result> UpdateUserPasswordAsync(UpdateUserPasswordRequest request, int userId);
        Task<Result> DeleteUserAsync(int userId);
        Task<Result> SendPasswordResetCodeAsync(UnverifiedUserRequest request);
        Task<Result> SendEmailConfirmationAsync(int userId);
        Task<Result<SignInUserResult>> ConfirmPasswordResetAsync(ResetPasswordRequest request);
        Task<Result> ConfirmUserEmailAsync(ConfirmUserEmailRequest request);
        Task<Result<AuthenticationTokensResult>> ReauthenticateUser(RefreshAccessTokenRequest request);
    }
}