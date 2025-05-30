using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetFiveApiDemo.Core.Common.DTOs;
using DotNetFiveApiDemo.Core.Common.DTOs.Base;
using DotNetFiveApiDemo.Core.User.DTOs;
using DotNetFiveApiDemo.Core.User.Entities;
using DotNetFiveApiDemo.Core.User.Errors;
using DotNetFiveApiDemo.Core.User.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace DotNetFiveApiDemo.Core.User.Services
{
    public class UserService : IUserService<AppUser>
    {
        private readonly ILogger<UserService> _logger;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;

        public UserService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            ILogger<UserService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<Result<AppUser>> GetUserAsync(int userId)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == userId);
            return user is not null
                ? Result.Success(user)
                : Result.Failure<AppUser>(UserError.UserNotFound(userId));
        }

        public async Task<Result<AppUser>> GetUserAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            return user is not null
                ? Result.Success(user)
                : Result.Failure<AppUser>(UserError.UserNotFound(email));
        }

        public async Task<Result<AppUser>> DeleteUserAsync(int userId)
        {
            var getUser = await GetUserAsync(userId);
            if (getUser.Failed) return Result.Failure<AppUser>(getUser.Error);

            var deleteUser = await _userManager.DeleteAsync(getUser.Value);
            if (deleteUser.Succeeded) return Result.Success<AppUser>();

            var errors = FormatIdentityErrors(deleteUser.Errors);
            _logger.LogInformation("Failed to delete user: {1}", errors);
            return Result.Failure<AppUser>(UserError.FailedToDeleteUser(errors));
        }

        public async Task<Result<string>> GeneratePasswordResetCodeAsync(string email)
        {
            var getUser = await GetUserAsync(email);
            if (getUser.Failed) return Result.Failure<string>(getUser.Error);
            var user = getUser.Value;

            var passwordResetCode = await _userManager.GeneratePasswordResetTokenAsync(user);
            return Result.Success(passwordResetCode);
        }

        public async Task<Result<string>> GenerateEmailConfirmationTokenAsync(AppUser user)
        {
            var emailConfirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            if (string.IsNullOrWhiteSpace(emailConfirmationToken))
                return Result.Failure<string>(UserError.FailedToGenerateEmailConfirmationToken);
            return Result.Success(emailConfirmationToken);
        }

        public async Task<Result<UpdateUserEmailResult>> UpdateUserEmailAsync(AppUser user, string newEmail)
        {
            if (newEmail.Equals(user.Email)) return Result.Failure<UpdateUserEmailResult>(UserError.EmailAlreadyInUse);

            var getUser = await GetUserAsync(newEmail);
            if (getUser.Succeeded) return Result.Failure<UpdateUserEmailResult>(UserError.EmailAlreadyInUse);

            user.Email = newEmail;
            user.UserName = GetEmailUsername(newEmail);
            var updateUser = await _userManager.UpdateAsync(user);
            if (updateUser.Succeeded) return Result.Success(new UpdateUserEmailResult(user.EmailConfirmed));

            var errors = FormatIdentityErrors(updateUser.Errors);
            _logger.LogInformation("Failed to update user email: {1}", errors);
            return Result.Failure<UpdateUserEmailResult>(UserError.FailedToUpdateEmail(errors));
        }

        public async Task<Result> UpdateUserPasswordAsync(AppUser user, string currentPassword, string newPassword)
        {
            var passwordsMatch = await _userManager.CheckPasswordAsync(user, currentPassword);
            if (!passwordsMatch)
                return Result.Failure(UserError.FailedToUpdateUserInfo("Current password is incorrect."));

            var updatePassword = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (!updatePassword.Succeeded)
            {
                var errors = FormatIdentityErrors(updatePassword.Errors);
                return Result.Failure(UserError.FailedToUpdateUserInfo(errors));
            }

            return Result.Success(user);
        }

        public async Task<Result> ResetPasswordAsync(string email, string resetCode, string newPassword)
        {
            var getUser = await GetUserAsync(email);
            if (getUser.Failed) return Result.Failure(getUser.Error);
            var user = getUser.Value;

            var userIsLockedOut = await _userManager.IsLockedOutAsync(user);
            if (userIsLockedOut) await _userManager.SetLockoutEnabledAsync(user, false);

            var resetPassword = await _userManager.ResetPasswordAsync(user, resetCode, newPassword);
            if (resetPassword.Succeeded) return Result.Success();

            var errors = FormatIdentityErrors(resetPassword.Errors);
            return Result.Failure(UserError.FailedToResetPassword(errors));
        }

        public async Task<Result> ConfirmEmailAsync(string email, string confirmationToken)
        {
            var getUser = await GetUserAsync(email);
            if (getUser.Failed) return Result.Failure(getUser.Error);
            var user = getUser.Value;

            var confirmEmail = await _userManager.ConfirmEmailAsync(user, confirmationToken);
            return confirmEmail.Succeeded
                ? Result.Success()
                : Result.Failure(UserError.FailedToConfirmUserEmail(user.Email));
        }

        public async Task<Result<AppUser>> CreateUserAsync(AppUser user)
        {
            var getUser = await GetUserAsync(user.Email);
            if (getUser.Succeeded)
                return Result.Failure<AppUser>(UserError.EmailAlreadyInUse);

            user.UserName = GetEmailUsername(user.Email);
            var createUser = await _userManager.CreateAsync(user, user.PasswordHash);
            if (!createUser.Succeeded)
            {
                var errors = FormatIdentityErrors(createUser.Errors);
                return Result.Failure<AppUser>(UserError.FailedToSignUpUser(errors));
            }

            user = await _userManager.FindByEmailAsync(user.Email);
            return Result.Success(user);
        }

        public async Task<Result<AppUser>> CheckCredentialsAsync(AppUser user)
        {
            // PasswordHash property is used to store value provided from the DTO after mapping.
            var unhashedPassword = user.PasswordHash;
            var email = user.Email;

            user = await _userManager.FindByEmailAsync(email);
            if (user is null) return Result.Failure<AppUser>(UserError.FailedToSignInUser);

            var signInResult = await _signInManager.CheckPasswordSignInAsync(user, unhashedPassword, true);
            if (signInResult.Succeeded)
            {
                var resetAccessFailedCount = await _userManager.ResetAccessFailedCountAsync(user);
                if (!resetAccessFailedCount.Succeeded)
                    throw new Exception("Failed to reset access failed count.");

                return Result.Success(user);
            }

            if (signInResult.IsLockedOut) return Result.Failure<AppUser>(UserError.UserIsLockedOut);

            if (signInResult.IsNotAllowed)
                throw new Exception("'IdentityOptions.SignIn.RequireConfirmedEmail' must be set to 'true'.");

            return Result.Failure<AppUser>(UserError.FailedToSignInUser);
        }

        public async Task<Result> UpdateUserAsync(AppUser user)
        {
            var updateUser = await _userManager.UpdateAsync(user);
            if (updateUser.Succeeded) return Result.Success();

            var errors = FormatIdentityErrors(updateUser.Errors);
            _logger.LogInformation("Failed to update user information: {1}", errors);
            return Result.Failure(UserError.FailedToUpdateUserInfo(errors));
        }

        private string FormatIdentityErrors(IEnumerable<IdentityError> errors)
        {
            // All IdentityError descriptions end with a period.
            return string.Join(" ", errors.Select(e => e.Description));
        }

        private string GetEmailUsername(string email)
        {
            return email[..email.IndexOf('@')];
        }
    }
}