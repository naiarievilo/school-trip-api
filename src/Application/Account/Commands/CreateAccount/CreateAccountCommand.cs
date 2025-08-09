using System.ComponentModel.DataAnnotations;
using Mediator;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Commands.CreateAccount;

public class CreateAccountCommand(
    string email,
    string password,
    string confirmPassword,
    string fullName)
    : ICommand<Result<CreateAccountResult>>
{
    [EmailAddress(ErrorMessage = "Email provided must be valid.")]
    public string Email { get; } = email;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; } = password;

    [Required(ErrorMessage = "Password confirmation is required.")]
    [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
    public string ConfirmPassword { get; } = confirmPassword;

    [Required(ErrorMessage = "Full name is required.")]
    public string FullName { get; } = fullName;
}