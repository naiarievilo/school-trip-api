using Mediator;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Accounts.Commands.DeleteAccount;

public sealed class DeleteAccountCommand(string accountId) : ICommand<Result>
{
    public string AccountId { get; set; } = accountId;

    public static DeleteAccountCommand For(string accountId)
    {
        return new DeleteAccountCommand(accountId);
    }
}