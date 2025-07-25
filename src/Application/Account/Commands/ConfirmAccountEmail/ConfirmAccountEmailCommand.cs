using System.ComponentModel.DataAnnotations;
using Mediator;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Commands.ConfirmAccountEmail;

public class ConfirmAccountEmailCommand(string email, string emailConfirmationToken) : ICommand<Result>
{
    [EmailAddress(ErrorMessage = "Email provided must be valid.")]
    public string Email { get; } = email;

    [Required(ErrorMessage = "Email confirmation token is required.")]
    public string EmailConfirmationToken { get; } = emailConfirmationToken;
}