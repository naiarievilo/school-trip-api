using Mediator;
using SchoolTripApi.Application.Account.Commands.AuthenticateAccount;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Commands.ResetAccountPassword;

public class ResetAccountPasswordHandler(IAccountManager accountManager, IAuthenticationService authenticationService)
    : ICommandHandler<ResetAccountPasswordCommand, Result<AuthenticateAccountResult>>
{
    public async ValueTask<Result<AuthenticateAccountResult>> Handle(ResetAccountPasswordCommand command,
        CancellationToken cancellationToken)
    {
        var resetAccountPassword = await accountManager.ResetAccountPasswordAsync(command.Email, command.ResetCode,
            command.NewPassword, cancellationToken);

        if (resetAccountPassword.Failed) return Result.Failure<AuthenticateAccountResult>(resetAccountPassword.Error);

        var getAccountInfo = await accountManager.GetAccountInfoAsync(command.Email, cancellationToken);
        if (getAccountInfo.Failed) return Result.Failure<AuthenticateAccountResult>(getAccountInfo.Error);
        var accountInfo = getAccountInfo.Value;

        var issueAuthenticationTokens = await authenticationService.IssueAuthenticationTokens(accountInfo.Id);
        if (issueAuthenticationTokens.Failed)
            return Result.Failure<AuthenticateAccountResult>(issueAuthenticationTokens.Error);
        var authenticationTokens = issueAuthenticationTokens.Value;

        return Result.Success(new AuthenticateAccountResult
        {
            AccountId = accountInfo.Id,
            IsEmailConfirmed = accountInfo.IsEmailConfirmed,
            AccessToken = authenticationTokens.AccessToken,
            ExpiresAt = authenticationTokens.ExpiresAt,
            RefreshToken = authenticationTokens.RefreshToken
        });
    }
}