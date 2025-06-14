using Mediator;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Application.Common.Security.DTOs;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Commands.ReauthenticateAccount;

public class ReauthenticateAccountHandler(IAuthenticationService authenticationService)
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