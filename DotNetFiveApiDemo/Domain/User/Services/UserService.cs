using System.Linq;
using System.Threading.Tasks;
using DotNetFiveApiDemo.Application.Common.DTOs;
using DotNetFiveApiDemo.Application.Common.DTOs.Base;
using DotNetFiveApiDemo.Application.User.Identity;
using DotNetFiveApiDemo.Domain.User.Errors;
using DotNetFiveApiDemo.Domain.User.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DotNetFiveApiDemo.Domain.User.Services
{
    public class UserService : IUserService<ApplicationUser>
    {
        // TODO: Integrate SignInManager to enable additional security features (e.g., user lockout, reset password, confirm email) 
        private readonly ILogger<UserService> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager, ILogger<UserService> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<Result<ApplicationUser>> GetUserAsync(int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user is not null
                ? Result.Success(user)
                : Result.Failure<ApplicationUser>(UserError.UserNotFound(userId));
        }

        public async Task<Result<ApplicationUser>> DeleteUserAsync(int userId)
        {
            var getUserResult = await GetUserAsync(userId);
            if (getUserResult.IsFailure) return Result.Failure<ApplicationUser>(UserError.UserNotFound(userId));

            var deleteUserResult = await _userManager.DeleteAsync(getUserResult.Value);
            if (deleteUserResult.Succeeded) return Result.Success<ApplicationUser>();

            var errors = FormatIdentityErrors(deleteUserResult);
            _logger.LogInformation(errors);
            return Result.Failure<ApplicationUser>(UserError.FailedToDeleteUser(errors));
        }

        public async Task<Result<ApplicationUser>> GetUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null
                ? Result.Success(user)
                : Result.Failure<ApplicationUser>(UserError.UserNotFound(email));
        }

        public async Task<Result<ApplicationUser>> CreateUserAsync(ApplicationUser user)
        {
            var result = await _userManager.CreateAsync(user, user.PasswordHash);
            if (!result.Succeeded)
            {
                var errors = FormatIdentityErrors(result);
                return Result.Failure<ApplicationUser>(UserError.FailedToCreateUser(errors));
            }

            user = await _userManager.FindByEmailAsync(user.Email);
            return Result.Success(user);
        }

        public async Task<Result<ApplicationUser>> ValidateCredentials(ApplicationUser user)
        {
            // PasswordHash property is used to store value provided from the DTO after mapping.
            var unhashedPassword = user.PasswordHash;
            var email = user.Email;
            var username = user.UserName;
            var loginField = "Username";

            if (email is not null)
            {
                loginField = "Email";
                user = await _userManager.FindByEmailAsync(email);
            }
            else
            {
                user = await _userManager.FindByNameAsync(username);
            }

            if (user is null) return Result.Failure<ApplicationUser>(UserError.FailedToLogInUser(loginField));

            var passwordsMatch = await _userManager.CheckPasswordAsync(user, unhashedPassword);
            return passwordsMatch
                ? Result.Success(user)
                : Result.Failure<ApplicationUser>(UserError.FailedToLogInUser(loginField));
        }

        public async Task<Result<ApplicationUser>> UpdateUserAsync(ApplicationUser user, string currentPassword = null,
            string newPassword = null)
        {
            var updateUser = await _userManager.UpdateAsync(user);
            if (!updateUser.Succeeded)
            {
                var errors = FormatIdentityErrors(updateUser);
                _logger.LogInformation(errors);
                return Result.Failure<ApplicationUser>(UserError.FailedToUpdateUser(errors));
            }

            if (string.IsNullOrWhiteSpace(currentPassword) && string.IsNullOrWhiteSpace(newPassword))
                return Result.Success(user);

            if (string.IsNullOrWhiteSpace(currentPassword) && !string.IsNullOrWhiteSpace(newPassword))
                return Result.Failure<ApplicationUser>(
                    UserError.FailedToUpdateUser("Current password is required to update password."));

            if (!string.IsNullOrWhiteSpace(currentPassword) && string.IsNullOrWhiteSpace(newPassword))
                return Result.Failure<ApplicationUser>(
                    UserError.FailedToUpdateUser("New password is required to update password."));

            var passwordsMatch = await _userManager.CheckPasswordAsync(user, currentPassword);
            if (!passwordsMatch)
                return Result.Failure<ApplicationUser>(UserError.FailedToUpdateUser("Current password is incorrect."));

            var updatePassword = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!updatePassword.Succeeded)
            {
                var errors = FormatIdentityErrors(updatePassword);
                _logger.LogInformation(errors);
                return Result.Failure<ApplicationUser>(UserError.FailedToUpdateUser(errors));
            }

            return Result.Success(user);
        }

        private string FormatIdentityErrors(IdentityResult result)
        {
            return string.Join(" ", result.Errors.Select(e => e.Description));
        }
    }
}