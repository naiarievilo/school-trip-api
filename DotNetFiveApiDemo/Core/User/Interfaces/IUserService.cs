using System.Threading.Tasks;
using DotNetFiveApiDemo.Core.Common.DTOs;
using DotNetFiveApiDemo.Core.Common.DTOs.Base;
using DotNetFiveApiDemo.Core.User.DTOs;

namespace DotNetFiveApiDemo.Core.User.Interfaces
{
    public interface IUserService<TUser> where TUser : class
    {
        Task<Result<TUser>> GetUserAsync(int userId);
        Task<Result<TUser>> GetUserAsync(string email);
        Task<Result<TUser>> CreateUserAsync(TUser user);
        Task<Result<TUser>> CheckCredentialsAsync(TUser user);
        Task<Result> UpdateUserAsync(TUser user);
        Task<Result<TUser>> DeleteUserAsync(int userId);
        Task<Result<string>> GeneratePasswordResetCodeAsync(string email);
        Task<Result<string>> GenerateEmailConfirmationTokenAsync(TUser user);
        Task<Result<UpdateUserEmailResult>> UpdateUserEmailAsync(TUser user, string newEmail);
        Task<Result> UpdateUserPasswordAsync(TUser user, string currentPassword, string newPassword);
        Task<Result> ResetPasswordAsync(string email, string resetCode, string newPassword);
        Task<Result> ConfirmEmailAsync(string email, string confirmationToken);
    }
}