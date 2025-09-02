using Mediator;
using SchoolTripApi.Application.Accounts.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Accounts.Commands.AuthenticateAccount;

public sealed class AuthenticateAccountHandler(
    IAuthenticationService authenticationService,
    IAccountManager accountManager)
    : ICommandHandler<AuthenticateAccountCommand, Result<AuthenticateAccountResult>>
{
    public async ValueTask<Result<AuthenticateAccountResult>> Handle(AuthenticateAccountCommand command,
        CancellationToken cancellationToken)
    {
        var email = command.Email;
        var password = command.Password;

        var authenticateUser = await authenticationService.CheckCredentialsAsync(email, password);
        if (authenticateUser.Failed) return Result.Failure<AuthenticateAccountResult>(authenticateUser.Error);

        var getUserAccountInfo = await accountManager.GetAccountInfoAsync(email, cancellationToken);
        if (getUserAccountInfo.Failed) return Result.Failure<AuthenticateAccountResult>(getUserAccountInfo.Error);

        var authenticationTokens = authenticateUser.Value;
        var userAccountInfo = getUserAccountInfo.Value;
        return Result.Success(new AuthenticateAccountResult
        {
            AccountId = userAccountInfo.Id,
            IsEmailConfirmed = userAccountInfo.IsEmailConfirmed,
            AccessToken = authenticationTokens.AccessToken,
            ExpiresAt = authenticationTokens.ExpiresAt,
            RefreshToken = authenticationTokens.RefreshToken
        });
    }
}