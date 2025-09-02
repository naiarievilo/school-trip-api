using Mediator;
using SchoolTripApi.Application.Accounts.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Application.Accounts.Commands.DeleteAccount;

public sealed class DeleteAccountHandler(IAccountManager accountManager) : ICommandHandler<DeleteAccountCommand, Result>
{
    public async ValueTask<Result> Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
    {
        var convertToAccountId = AccountId.TryFrom(command.AccountId);
        if (convertToAccountId.Failed) return Result.Failure(convertToAccountId.Error);
        var accountId = convertToAccountId.Value;

        var deleteAccount = await accountManager.DeleteAccountAsync(accountId, cancellationToken);
        return deleteAccount.Succeeded
            ? Result.Success()
            : Result.Failure(deleteAccount.Error);
    }
}