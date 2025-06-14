using Mediator;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Commands.UpdateAccountPassword;

public class UpdateAccountPasswordHandler(IAccountManager accountManager)
    : ICommandHandler<UpdateAccountPasswordCommand, Result>
{
    public async ValueTask<Result> Handle(UpdateAccountPasswordCommand command, CancellationToken cancellationToken)
    {
        var updateAccountPassword = await accountManager.UpdateAccountPasswordAsync(command.AccountId,
            command.CurrentPassword, command.NewPassword, cancellationToken);
        return updateAccountPassword.Succeeded
            ? Result.Success()
            : Result.Failure(updateAccountPassword.Error);
    }
}