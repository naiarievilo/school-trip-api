using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using DotNetFiveApiDemo.Core.Common.DTOs.Base;
using DotNetFiveApiDemo.Core.Email.Errors;
using DotNetFiveApiDemo.Core.Email.Interfaces;
using DotNetFiveApiDemo.Core.Email.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace DotNetFiveApiDemo.Infrastructure.Email.Services
{
    internal class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(IOptions<EmailSettings> mailingSettings, ILogger<EmailSender> logger)
        {
            _emailSettings = mailingSettings.Value;
            _logger = logger;
        }

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
                _logger.LogError(ex, $"Error sending email to '{email}'.");
                return Result.Failure(EmailError.EmailNotSent);
            }
        }
    }
}