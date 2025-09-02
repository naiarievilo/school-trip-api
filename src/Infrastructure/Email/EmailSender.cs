using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;
using SchoolTripApi.Application.Common.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Infrastructure.Email;

internal sealed class EmailSender(IOptions<EmailSettings> mailingSettings, IAppLogger<EmailSender> logger)
    : IEmailSender
{
    private readonly EmailSettings _emailSettings = mailingSettings.Value;

    public async Task<Result> SendEmailAsync(string email, string subject, string htmlMessage)
    {
        try
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            using var smtpClient = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort);
            smtpClient.Credentials =
                new NetworkCredential(_emailSettings.SmtpUsername, _emailSettings.SmtpPassword);
            smtpClient.EnableSsl = _emailSettings.EnableSsl;

            await smtpClient.SendMailAsync(mailMessage);
            return Result.Success();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Error sending email to '{email}'.");
            return Result.Failure(EmailError.EmailNotSent);
        }
    }
}