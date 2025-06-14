using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Mediator;
using SchoolTripApi.Domain.Common.DTOs;
using AccountId = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.AccountId;

namespace SchoolTripApi.Application.Account.Commands.UpdateAccountPassword;

public class UpdateAccountPasswordCommand(string currentPassword, string newPassword, string confirmNewPassword)
    : ICommand<Result>
{
    [Required(ErrorMessage = "Current password is required.")]
    public string CurrentPassword { get; } = currentPassword;

    [Required(ErrorMessage = "New password is required.")]
    public string NewPassword { get; } = newPassword;

    [Required(ErrorMessage = "New password confirmation is required.")]
    [Compare(nameof(NewPassword), ErrorMessage = "New passwords do not match.")]
    public string ConfirmNewPassword { get; } = confirmNewPassword;

    [JsonIgnore] public AccountId AccountId { get; private set; }

    public UpdateAccountPasswordCommand For(AccountId accountId)
    {
        AccountId = accountId;
        return this;
    }
}