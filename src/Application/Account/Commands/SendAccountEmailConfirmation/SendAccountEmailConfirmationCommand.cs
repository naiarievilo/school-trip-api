using Mediator;
using SchoolTripApi.Domain.Common.DTOs;
using AccountId = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.AccountId;

namespace SchoolTripApi.Application.Account.Commands.SendAccountEmailConfirmation;

public class SendAccountEmailConfirmationCommand : ICommand<Result>
{
    private SendAccountEmailConfirmationCommand(AccountId accountId)
    {
        AccountId = accountId;
    }

    public AccountId AccountId { get; private set; }

    public static SendAccountEmailConfirmationCommand For(AccountId accountId)
    {
        return new SendAccountEmailConfirmationCommand(accountId);
    }
}