using System.ComponentModel.DataAnnotations;
using Mediator;
using SchoolTripApi.Application.Account.Commands.AuthenticateAccount;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Commands.ResetAccountPassword;

public class ResetAccountPasswordCommand(string email, string resetCode, string newPassword, string confirmNewPassword)
    : ICommand<Result<AuthenticateAccountResult>>
{
    [EmailAddress(ErrorMessage = "Email provided must be valid.")]
    public string Email { get; } = email;

    [Required(ErrorMessage = "Reset code is required.")]
    public string ResetCode { get; } = resetCode;

    [Required(ErrorMessage = "Password is required.")]
    [DataType(DataType.Password)]
    public string NewPassword { get; } = newPassword;

    [Required(ErrorMessage = "New password confirmation is required.")]
    [DataType(DataType.Password)]
    [Compare("NewPassword", ErrorMessage = "Passwords do not match.")]
    public string ConfirmNewPassword { get; } = confirmNewPassword;
}