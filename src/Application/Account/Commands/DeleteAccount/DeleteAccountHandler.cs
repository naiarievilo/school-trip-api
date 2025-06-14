using Mediator;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Commands.DeleteAccount;

public class DeleteAccountHandler(IAccountManager accountManager) : ICommandHandler<DeleteAccountCommand, Result>
{
    public async ValueTask<Result> Handle(DeleteAccountCommand command, CancellationToken cancellationToken)
    {
        var deleteAccount = await accountManager.DeleteAccountAsync(command.AccountId, cancellationToken);
        return deleteAccount.Succeeded
            ? Result.Success()
            : Result.Failure(deleteAccount.Error);
    }
}