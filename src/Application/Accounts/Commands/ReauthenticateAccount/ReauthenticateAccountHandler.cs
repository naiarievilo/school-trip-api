using Mediator;
using SchoolTripApi.Application.Accounts.Abstractions;
using SchoolTripApi.Application.Accounts.DTOs;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Accounts.Commands.ReauthenticateAccount;

public sealed class ReauthenticateAccountHandler(IAuthenticationService authenticationService)
    : ICommandHandler<ReauthenticateAccountCommand, Result<AuthenticationTokensResult>>
{
    public async ValueTask<Result<AuthenticationTokensResult>> Handle(ReauthenticateAccountCommand command,
        CancellationToken cancellationToken)
    {
        var reauthenticateUser = await authenticationService.RefreshAuthenticationTokensAsync(command.RefreshToken);
        return reauthenticateUser.Succeeded
            ? Result.Success(reauthenticateUser.Value)
            : Result.Failure<AuthenticationTokensResult>(reauthenticateUser.Error);
    }
}