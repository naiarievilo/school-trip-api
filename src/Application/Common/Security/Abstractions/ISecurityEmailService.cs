using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Common.Security.Abstractions;

public interface ISecurityEmailService
{
    Task<Result> SendEmailConfirmationLinkAsync(string email, string emailConfirmationToken);
    Task<Result> SendPasswordResetCodeAsync(string email, string resetCode);
    Task<Result> SendUnlockUserEmailAsync(string email, string passwordResetCode);
}