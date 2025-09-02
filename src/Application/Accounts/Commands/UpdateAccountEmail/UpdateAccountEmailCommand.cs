using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Mediator;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Accounts.Commands.UpdateAccountEmail;

public sealed class UpdateAccountEmailCommand(string newEmail) : ICommand<Result<UpdateAccountEmailResult>>
{
    [EmailAddress(ErrorMessage = "New email provided must be valid.")]
    public string NewEmail { get; } = newEmail;

    [JsonIgnore] public string? AccountId { get; private set; }

    public UpdateAccountEmailCommand For(string accountId)
    {
        AccountId = accountId;
        return this;
    }
}