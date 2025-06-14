using Mediator;
using SchoolTripApi.Domain.Common.DTOs;
using AccountId = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.AccountId;

namespace SchoolTripApi.Application.Account.Commands.DeleteAccount;

public class DeleteAccountCommand : ICommand<Result>
{
    private DeleteAccountCommand(AccountId accountId)
    {
        AccountId = accountId;
    }

    public AccountId AccountId { get; private set; }

    public static DeleteAccountCommand For(AccountId accountId)
    {
        return new DeleteAccountCommand(accountId);
    }
}