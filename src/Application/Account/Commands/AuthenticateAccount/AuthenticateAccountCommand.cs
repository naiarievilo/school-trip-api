using System.ComponentModel.DataAnnotations;
using Mediator;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Commands.AuthenticateAccount;

public class AuthenticateAccountCommand(string email, string password)
    : ICommand<Result<AuthenticateAccountResult>>
{
    [EmailAddress(ErrorMessage = "Email provided must be valid.")]
    public string Email { get; } = email;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; } = password;
}