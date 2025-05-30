using System;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using DotNetFiveApiDemo.Core.Common.DTOs;
using DotNetFiveApiDemo.Core.Common.DTOs.Base;
using DotNetFiveApiDemo.Core.Email.Interfaces;
using DotNetFiveApiDemo.Core.Email.Settings;
using DotNetFiveApiDemo.Core.Security.DTOs;
using DotNetFiveApiDemo.Core.Security.Errors;
using DotNetFiveApiDemo.Core.Security.Interfaces;
using DotNetFiveApiDemo.Core.Security.Settings;
using DotNetFiveApiDemo.Core.User.Entities;
using DotNetFiveApiDemo.WebApi.User.DTOs;
using Microsoft.Extensions.Options;

namespace DotNetFiveApiDemo.Core.Security.Services
{
    internal class AuthenticationService : IAuthenticationService<AppUser>
    {
        private readonly ClientUrls _clientUrls;
        private readonly IEmailSender _emailSender;
        private readonly EmailSettings _emailSettings;
        private readonly HtmlEncoder _htmlEncoder = HtmlEncoder.Default;
        private readonly IJwtTokenProvider _jwtTokenProvider;

        public AuthenticationService(IJwtTokenProvider jwtTokenProvider, IEmailSender emailSender,
            IOptions<ClientSettings> clientSettings, IOptions<EmailSettings> mailingSettings)
        {
            _jwtTokenProvider = jwtTokenProvider;
            _emailSender = emailSender;
            _clientUrls = clientSettings.Value.Urls;
            _emailSettings = mailingSettings.Value;
        }

        public async Task<Result<AuthenticationTokensResult>> IssueAuthenticationTokensAsync(
            RefreshAccessTokenRequest request)
        {
            var refreshToken = request.RefreshToken;
            return await _jwtTokenProvider.RefreshAccessTokenAsync(refreshToken);
        }

        public async Task<Result> SendEmailConfirmationLinkAsync(AppUser user, string emailConfirmationToken)
        {
            var emailConfirmationPage = _clientUrls.ConfirmEmail;
            if (string.IsNullOrWhiteSpace(emailConfirmationPage))
                return Result.Failure(SecurityError.ClientUrlNotProvided);

            var userEmail = user.Email;
            var subject = "Confirm your email";
            var confirmationLink =
                _htmlEncoder.Encode($"{emailConfirmationPage}?email={userEmail}&token={emailConfirmationToken}");

            var htmlContent = @$"
                <p>
                    Please confirm your account by clicking this <a href='{confirmationLink}'>link</a>.
                </p>
                <p>
                    If you did not create this account, please ignore this email.
                </p>
            ";

            return await _emailSender.SendEmailAsync(userEmail, subject, htmlContent);
        }

        public async Task<Result> SendPasswordResetCodeAsync(AppUser user, string resetCode)
        {
            var passwordResetPage = _clientUrls.ResetPassword;
            if (string.IsNullOrWhiteSpace(passwordResetPage))
                return Result.Failure(SecurityError.ClientUrlNotProvided);

            var userEmail = user.Email;
            var subject = "Reset password";
            var resetLink = _htmlEncoder.Encode($"{passwordResetPage}?email={userEmail}&code={resetCode}");

            var htmlContent = $@"
                <p>
                    To reset your password, click this <a href='{resetLink}'>link</a>.
                </p>
                <p>
                    If you did not request a password reset, please ignore this email.
                </p>
            ";

            return await _emailSender.SendEmailAsync(userEmail, subject, htmlContent);
        }

        public async Task<Result> SendUnlockUserEmailAsync(string email, string passwordResetCode)
        {
            var passwordResetPage = _clientUrls.ResetPassword;
            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(passwordResetCode))
                throw new ArgumentException("'email' and 'unlockUserToken' are required.");

            var userEmail = email;
            var supportEmail = _emailSettings.SupportEmail ?? "[Support email here]";
            var subject = "Unlock your account";
            var unlockUserLink = _htmlEncoder.Encode($"{passwordResetPage}?email={userEmail}&code={passwordResetCode}");

            var htmlContent = $@"
                <p>
                    Your account has been temporarily locked to protect your security after too many unsuccessful sign-in attempts were detected.
                </p>
                <p>
                    To unlock your account, please reset your password by clicking this <a href={unlockUserLink}>link</a> or wait 30 minutes before trying again.
                </p>
                <p>
                    If you did not attempt to sign in recently, or if you have any concerns about your account's security, please contact us immediately at {supportEmail}.
                </p>
            ";

            return await _emailSender.SendEmailAsync(email, subject, htmlContent);
        }

        public async Task<Result<AuthenticationTokensResult>> IssueAuthenticationTokensAsync(AppUser user)
        {
            var userId = user.Id;

            var issueAccessToken = _jwtTokenProvider.IssueAccessToken(userId);
            var issueRefreshToken = await _jwtTokenProvider.IssueRefreshTokenAsync(userId);
            if (issueRefreshToken.Failed)
                return Result.Failure<AuthenticationTokensResult>(issueRefreshToken.Error);

            return Result.Success(new AuthenticationTokensResult
            {
                AccessToken = issueAccessToken.AccessToken,
                ExpiresAt = issueAccessToken.ExpiresAt,
                RefreshToken = issueRefreshToken.Value
            });
        }
    }
}