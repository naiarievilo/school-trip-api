using Mediator;
using SchoolTripApi.Application.Accounts.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Application.Accounts.Commands.UpdateAccountEmail;

public sealed class UpdateAccountEmailHandler(IAccountManager accountManager)
    : ICommandHandler<UpdateAccountEmailCommand, Result<UpdateAccountEmailResult>>
{
    public async ValueTask<Result<UpdateAccountEmailResult>> Handle(UpdateAccountEmailCommand command,
        CancellationToken cancellationToken)
    {
        var convertToAccountId = AccountId.TryFrom(command.AccountId);
        if (convertToAccountId.Failed) return Result.Failure<UpdateAccountEmailResult>(convertToAccountId.Error);
        var accountId = convertToAccountId.Value;

        var updateAccountEmail =
            await accountManager.UpdateAccountEmailAsync(accountId, command.NewEmail, cancellationToken);
        return updateAccountEmail.Succeeded
            ? Result.Success(updateAccountEmail.Value)
            : Result.Failure<UpdateAccountEmailResult>(updateAccountEmail.Error);
    }
}