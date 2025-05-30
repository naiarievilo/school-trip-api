using System.Threading.Tasks;
using AutoMapper;
using DotNetFiveApiDemo.Application.User.DTOs;
using DotNetFiveApiDemo.Application.User.Interfaces;
using DotNetFiveApiDemo.Core.Common.DTOs;
using DotNetFiveApiDemo.Core.Common.DTOs.Base;
using DotNetFiveApiDemo.Core.Security.DTOs;
using DotNetFiveApiDemo.Core.Security.Interfaces;
using DotNetFiveApiDemo.Core.User.DTOs;
using DotNetFiveApiDemo.Core.User.Entities;
using DotNetFiveApiDemo.Core.User.Errors;
using DotNetFiveApiDemo.Core.User.Interfaces;
using DotNetFiveApiDemo.WebApi.User.DTOs;
using Microsoft.Extensions.Logging;

namespace DotNetFiveApiDemo.Application.User.Services
{
    public class UserApplicationService : IUserApplicationService
    {
        private readonly IAuthenticationService<AppUser> _authenticationService;
        private readonly ILogger<UserApplicationService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserService<AppUser> _userService;

        public UserApplicationService(IUserService<AppUser> userService, ILogger<UserApplicationService> logger,
            IAuthenticationService<AppUser> authenticationService, IMapper mapper)
        {
            _userService = userService;
            _authenticationService = authenticationService;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<Result<SignUpUserResult>> SignUpUserAsync(SignUpUserRequest request)
        {
            var user = _mapper.Map<AppUser>(request);

            var createUser = await _userService.CreateUserAsync(user);
            if (createUser.Failed) return Result.Failure<SignUpUserResult>(createUser.Error);

            var issueAuthenticationTokens = await _authenticationService.IssueAuthenticationTokensAsync(user);
            if (issueAuthenticationTokens.Failed)
                return Result.Failure<SignUpUserResult>(issueAuthenticationTokens.Error);

            var newUser = createUser.Value;
            var authTokens = issueAuthenticationTokens.Value;

            return Result.Success(new SignUpUserResult
            {
                UserId = newUser.Id,
                IsEmailConfirmed = newUser.EmailConfirmed,
                AccessToken = authTokens.AccessToken,
                RefreshToken = authTokens.RefreshToken
            });
        }

        public async Task<Result<UserDto>> GetUserAsync(int userId)
        {
            var getUser = await _userService.GetUserAsync(userId);
            if (getUser.Failed) return Result.Failure<UserDto>(getUser.Error);

            var userDto = _mapper.Map<UserDto>(getUser.Value);
            return Result.Success(userDto);
        }

        public async Task<Result<SignInUserResult>> SignInUserAsync(SignInUserRequest request)
        {
            var user = _mapper.Map<AppUser>(request);

            var checkCredentials = await _userService.CheckCredentialsAsync(user);
            if (checkCredentials.Succeeded)
            {
                user = checkCredentials.Value;

                var issueAuthenticationTokens = await _authenticationService.IssueAuthenticationTokensAsync(user);
                if (issueAuthenticationTokens.Failed)
                    return Result.Failure<SignInUserResult>(issueAuthenticationTokens.Error);

                var tokens = issueAuthenticationTokens.Value;
                return Result.Success(new SignInUserResult
                {
                    UserId = user.Id,
                    AccessToken = tokens.AccessToken,
                    ExpiresAt = tokens.ExpiresAt,
                    RefreshToken = tokens.RefreshToken
                });
            }

            var email = user.Email;
            var getUser = await _userService.GetUserAsync(email);
            if (getUser.Failed) return Result.Failure<SignInUserResult>(UserError.FailedToSignInUser);
            user = getUser.Value;

            if (checkCredentials.Error.Code.Equals(UserError.UserIsLockedOutCode) && !user.UnlockMessageSent)
            {
                var generatePasswordResetCode = await _userService.GeneratePasswordResetCodeAsync(email);
                if (generatePasswordResetCode.Failed)
                    return Result.Failure<SignInUserResult>(UserError.FailedToSignInUser);

                var resetCode = generatePasswordResetCode.Value;
                await _authenticationService.SendUnlockUserEmailAsync(email, resetCode);

                user.UnlockMessageSent = true;
                await _userService.UpdateUserAsync(user);

                return Result.Failure<SignInUserResult>(UserError.FailedToSignInUser);
            }

            return Result.Failure<SignInUserResult>(checkCredentials.Error);
        }

        public async Task<Result> UpdateUserInfoAsync(UpdateUserInfoRequest request, int userId)
        {
            var getUser = await _userService.GetUserAsync(userId);
            if (getUser.Failed) return Result.Failure<UserDto>(getUser.Error);

            var userToUpdate = _mapper.Map(request, getUser.Value);
            var updateUser = await _userService.UpdateUserAsync(userToUpdate);

            return updateUser.Succeeded
                ? Result.Success()
                : Result.Failure<UserDto>(updateUser.Error);
        }

        public async Task<Result<UpdateUserEmailResult>> UpdateUserEmailAsync(UpdateUserEmailRequest request,
            int userId)
        {
            var getUser = await _userService.GetUserAsync(userId);
            if (getUser.Failed) return Result.Failure<UpdateUserEmailResult>(getUser.Error);
            var user = getUser.Value;

            var updateUserEmail = await _userService.UpdateUserEmailAsync(user, request.NewEmail);
            return updateUserEmail.Succeeded
                ? Result.Success(new UpdateUserEmailResult(false))
                : Result.Failure<UpdateUserEmailResult>(updateUserEmail.Error);
        }

        public async Task<Result> UpdateUserPasswordAsync(UpdateUserPasswordRequest request, int userId)
        {
            var currentPassword = request.CurrentPassword;
            var newPassword = request.NewPassword;

            var getUser = await _userService.GetUserAsync(userId);
            if (getUser.Failed) return Result.Failure(getUser.Error);
            var user = getUser.Value;

            var updateUserPassword = await _userService.UpdateUserPasswordAsync(user, currentPassword, newPassword);
            return updateUserPassword.Succeeded
                ? Result.Success()
                : Result.Failure(updateUserPassword.Error);
        }

        public async Task<Result> DeleteUserAsync(int userId)
        {
            var userDeletion = await _userService.DeleteUserAsync(userId);
            return userDeletion.Succeeded ? Result.Success() : Result.Failure(userDeletion.Error);
        }

        public async Task<Result> SendPasswordResetCodeAsync(UnverifiedUserRequest request)
        {
            var getUser = await _userService.GetUserAsync(request.Email);
            if (getUser.Failed) return Result.Success();
            var user = getUser.Value;

            var generatePasswordReset = await _userService.GeneratePasswordResetCodeAsync(user.Email);
            if (generatePasswordReset.Failed)
            {
                _logger.LogError(generatePasswordReset.Error);
                return Result.Success();
            }

            var passwordResetCode = generatePasswordReset.Value;
            var sendPasswordResetLink =
                await _authenticationService.SendPasswordResetCodeAsync(user, passwordResetCode);
            if (sendPasswordResetLink.Succeeded) return Result.Success();

            _logger.LogWarning(sendPasswordResetLink.Error);
            return Result.Success();
        }

        public async Task<Result> SendEmailConfirmationAsync(int userId)
        {
            var getUser = await _userService.GetUserAsync(userId);
            if (getUser.Failed) return Result.Success();
            var user = getUser.Value;

            var generateEmailConfirmationToken = await _userService.GenerateEmailConfirmationTokenAsync(user);
            if (generateEmailConfirmationToken.Failed) return Result.Failure(generateEmailConfirmationToken.Error);
            var emailConfirmationToken = generateEmailConfirmationToken.Value;

            return await _authenticationService.SendEmailConfirmationLinkAsync(user, emailConfirmationToken);
        }

        public async Task<Result<SignInUserResult>> ConfirmPasswordResetAsync(ResetPasswordRequest request)
        {
            var email = request.Email;
            var resetCode = request.ResetCode;
            var newPassword = request.NewPassword;

            var getUser = await _userService.GetUserAsync(email);
            if (getUser.Failed) return Result.Failure<SignInUserResult>(getUser.Error);
            var user = getUser.Value;

            var confirmPasswordReset = await _userService.ResetPasswordAsync(email, resetCode, newPassword);
            if (confirmPasswordReset.Failed) return Result.Failure<SignInUserResult>(confirmPasswordReset.Error);

            user.UnlockMessageSent = false;
            await _userService.UpdateUserAsync(user);

            var issueAuthenticationTokens = await _authenticationService.IssueAuthenticationTokensAsync(user);
            if (issueAuthenticationTokens.Failed)
                return Result.Failure<SignInUserResult>(issueAuthenticationTokens.Error);


            var tokens = issueAuthenticationTokens.Value;
            return Result.Success(new SignInUserResult
            {
                UserId = user.Id,
                AccessToken = tokens.AccessToken,
                ExpiresAt = tokens.ExpiresAt,
                RefreshToken = tokens.RefreshToken
            });
        }

        public async Task<Result> ConfirmUserEmailAsync(ConfirmUserEmailRequest request)
        {
            var email = request.Email;
            var emailConfirmationToken = request.EmailConfirmationToken;
            return await _userService.ConfirmEmailAsync(email, emailConfirmationToken);
        }

        public async Task<Result<AuthenticationTokensResult>> ReauthenticateUser(RefreshAccessTokenRequest request)
        {
            var issueAuthenticationTokens = await _authenticationService.IssueAuthenticationTokensAsync(request);
            return issueAuthenticationTokens.Succeeded
                ? Result.Success(issueAuthenticationTokens.Value)
                : Result.Failure<AuthenticationTokensResult>(issueAuthenticationTokens.Error);
        }
    }
}