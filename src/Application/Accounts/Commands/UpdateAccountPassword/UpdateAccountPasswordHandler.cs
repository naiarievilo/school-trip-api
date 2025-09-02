using Mediator;
using SchoolTripApi.Application.Accounts.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Application.Accounts.Commands.UpdateAccountPassword;

public sealed class UpdateAccountPasswordHandler(IAccountManager accountManager)
    : ICommandHandler<UpdateAccountPasswordCommand, Result>
{
    public async ValueTask<Result> Handle(UpdateAccountPasswordCommand command, CancellationToken cancellationToken)
    {
        var convertToAccountId = AccountId.TryFrom(command.AccountId);
        if (convertToAccountId.Failed) return Result.Failure(convertToAccountId.Error);
        var accountId = convertToAccountId.Value;

        var updateAccountPassword = await accountManager.UpdateAccountPasswordAsync(accountId,
            command.CurrentPassword, command.NewPassword, cancellationToken);
        return updateAccountPassword.Succeeded
            ? Result.Success()
            : Result.Failure(updateAccountPassword.Error);
    }
}