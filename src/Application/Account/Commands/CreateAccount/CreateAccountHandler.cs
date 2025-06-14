using Mediator;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Commands.CreateAccount;

public class CreateAccountHandler(
    IAccountManager accountManager,
    IAuthenticationService authenticationService)
    : ICommandHandler<CreateAccountCommand, Result<CreateAccountResult>>
{
    public async ValueTask<Result<CreateAccountResult>> Handle(CreateAccountCommand command,
        CancellationToken cancellationToken)
    {
        var createUserAccount = await accountManager.CreateAccountAsync(command, cancellationToken);
        if (createUserAccount.Failed) return Result.Failure<CreateAccountResult>(createUserAccount.Error);
        var userAccountInfo = createUserAccount.Value;

        var issueAuthenticationTokens = await authenticationService.IssueAuthenticationTokens(userAccountInfo.Id);
        if (issueAuthenticationTokens.Failed)
            return Result.Failure<CreateAccountResult>(issueAuthenticationTokens.Error);
        var authenticationTokens = issueAuthenticationTokens.Value;

        return Result.Success(new CreateAccountResult
        {
            AccountId = userAccountInfo.Id,
            IsEmailConfirmed = userAccountInfo.IsEmailConfirmed,
            AccessToken = authenticationTokens.AccessToken,
            ExpiresAt = authenticationTokens.ExpiresAt,
            RefreshToken = authenticationTokens.RefreshToken
        });
    }
}