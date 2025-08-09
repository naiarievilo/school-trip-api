using Mediator;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Commands.DeleteAccount;

public class DeleteAccountCommand : ICommand<Result>
{
    private DeleteAccountCommand(string accountId)
    {
        AccountId = accountId;
    }

    public string AccountId { get; private set; }

    public static DeleteAccountCommand For(string accountId)
    {
        return new DeleteAccountCommand(accountId);
    }
}