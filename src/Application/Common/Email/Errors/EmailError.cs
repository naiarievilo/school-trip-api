using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Common.Email.Errors
{
    public class EmailError : Error
    {
        private const string EmailNotSentCode = "MailError.EmailNotSent";

        private EmailError(string code, string description) : base(code, description)
        {
        }

        public static Error EmailNotSent()
        {
            return new EmailError(EmailNotSentCode, "Failed to send email. Try again later.");
        }
    }
}