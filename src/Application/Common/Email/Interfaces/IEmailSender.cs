using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Common.Email.Interfaces;

public interface IEmailSender
{
    public Task<Result> SendEmailAsync(string email, string subject, string htmlMessage);
}