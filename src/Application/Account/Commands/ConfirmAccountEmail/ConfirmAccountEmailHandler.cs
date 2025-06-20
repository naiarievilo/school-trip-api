using Mediator;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Commands.ConfirmAccountEmail;

public class ConfirmAccountEmailHandler(IAccountManager accountManager)
    : ICommandHandler<ConfirmAccountEmailCommand, Result>
{
    public async ValueTask<Result> Handle(ConfirmAccountEmailCommand command,
        CancellationToken cancellationToken)
    {
        var confirmEmail = await accountManager.ConfirmAccountEmailAsync(command.Email, command.EmailConfirmationToken,
            cancellationToken);

        return confirmEmail.Succeeded
            ? Result.Success()
            : Result.Failure(confirmEmail.Error);
    }
}