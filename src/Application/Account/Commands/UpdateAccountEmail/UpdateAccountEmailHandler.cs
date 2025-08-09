using Mediator;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Application.Account.Commands.UpdateAccountEmail;

public class UpdateAccountEmailHandler(IAccountManager accountManager)
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