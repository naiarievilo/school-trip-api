namespace SchoolTripApi.Application.Common.Security.DTOs;

public class GeneratePasswordResetCodeResult(string passwordResetCode)
{
    public required string PasswordResetCode { get; init; } = passwordResetCode;
}