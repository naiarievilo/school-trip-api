using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Mediator;
using SchoolTripApi.Domain.Common.DTOs;
using AccountId = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.AccountId;

namespace SchoolTripApi.Application.Account.Commands.UpdateAccountEmail;

public class UpdateAccountEmailCommand(string newEmail) : ICommand<Result<UpdateAccountEmailResult>>
{
    [EmailAddress(ErrorMessage = "New email provided must be valid.")]
    public string NewEmail { get; } = newEmail;

    [JsonIgnore] public AccountId AccountId { get; private set; }

    public UpdateAccountEmailCommand For(AccountId accountId)
    {
        AccountId = accountId;
        return this;
    }
}