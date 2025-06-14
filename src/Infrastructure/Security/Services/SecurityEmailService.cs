using System.Text.Encodings.Web;
using Microsoft.Extensions.Options;
using SchoolTripApi.Application.Common.Email.Interfaces;
using SchoolTripApi.Application.Common.Email.Settings;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Application.Common.Security.Errors;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Infrastructure.Security.Settings;

namespace SchoolTripApi.Infrastructure.Security.Services;

internal class SecurityEmailService(
    IEmailSender emailSender,
    IOptions<ClientSettings> clientSettings,
    IOptions<EmailSettings> mailingSettings)
    : ISecurityEmailService
{
    private readonly ClientUrls _clientUrls = clientSettings.Value.Urls;
    private readonly EmailSettings _emailSettings = mailingSettings.Value;
    private readonly HtmlEncoder _htmlEncoder = HtmlEncoder.Default;

    public async Task<Result> SendUnlockUserEmailAsync(string email, string passwordResetCode)
    {
        var passwordResetPage = _clientUrls.ResetPassword;
        if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(passwordResetCode))
            throw new ArgumentException("'email' and 'unlockUserToken' are required.");

        var userEmail = email;
        var supportEmail = _emailSettings.SupportEmail;
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

        return await emailSender.SendEmailAsync(email, subject, htmlContent);
    }

    public async Task<Result> SendEmailConfirmationLinkAsync(string email, string emailConfirmationToken)
    {
        var emailConfirmationPage = _clientUrls.ConfirmEmail;
        if (string.IsNullOrWhiteSpace(emailConfirmationPage))
            return Result.Failure(SecurityError.ClientUrlNotProvided);

        var subject = "Confirm your email";
        var confirmationLink =
            $"{emailConfirmationPage}?email={_htmlEncoder.Encode(email)}&token={emailConfirmationToken}";

        var htmlContent = @$"
                <p>
                    Please confirm your account by clicking this <a href='{confirmationLink}'>link</a>.
                </p>
                <p>
                    If you did not create this account, please ignore this email.
                </p>
            ";

        return await emailSender.SendEmailAsync(email, subject, htmlContent);
    }

    public async Task<Result> SendPasswordResetCodeAsync(string email, string resetCode)
    {
        var passwordResetPage = _clientUrls.ResetPassword;
        if (string.IsNullOrWhiteSpace(passwordResetPage))
            return Result.Failure(SecurityError.ClientUrlNotProvided);

        var subject = "Reset password";
        var resetLink = $"{passwordResetPage}?email={_htmlEncoder.Encode(email)}&code={resetCode}";

        var htmlContent = $@"
                <p>
                    To reset your password, click this <a href='{resetLink}'>link</a>.
                </p>
                <p>
                    If you did not request a password reset, please ignore this email.
                </p>
            ";

        return await emailSender.SendEmailAsync(email, subject, htmlContent);
    }
}