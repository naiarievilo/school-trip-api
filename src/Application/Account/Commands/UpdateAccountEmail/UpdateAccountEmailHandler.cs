using Mediator;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Commands.UpdateAccountEmail;

public class UpdateAccountEmailHandler(IAccountManager accountManager)
    : ICommandHandler<UpdateAccountEmailCommand, Result<UpdateAccountEmailResult>>
{
    public async ValueTask<Result<UpdateAccountEmailResult>> Handle(UpdateAccountEmailCommand command,
        CancellationToken cancellationToken)
    {
        var updateAccountEmail =
            await accountManager.UpdateAccountEmailAsync(command.AccountId, command.NewEmail, cancellationToken);
        return updateAccountEmail.Succeeded
            ? Result.Success(updateAccountEmail.Value)
            : Result.Failure<UpdateAccountEmailResult>(updateAccountEmail.Error);
    }
}