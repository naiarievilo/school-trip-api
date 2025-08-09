using Mediator;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Application.Account.Commands.UpdateAccountPassword;

public class UpdateAccountPasswordHandler(IAccountManager accountManager)
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