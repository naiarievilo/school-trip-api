using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Infrastructure.Email;

public interface IEmailSender
{
    public Task<Result> SendEmailAsync(string email, string subject, string htmlMessage);
}