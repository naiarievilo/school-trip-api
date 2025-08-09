using Mediator;
using SchoolTripApi.Application.Common.Abstractions;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Guardian.GuardianAggregate;
using SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Application.Account.Commands.CreateAccount;

public class CreateAccountHandler(
    IAccountManager accountManager,
    IAuthenticationService authenticationService,
    IRepository<Guardian> guardianRepository)
    : ICommandHandler<CreateAccountCommand, Result<CreateAccountResult>>
{
    public async ValueTask<Result<CreateAccountResult>> Handle(CreateAccountCommand command,
        CancellationToken cancellationToken)
    {
        var email = command.Email;
        var password = command.Password;

        var convertGuardianFullName = FullName.TryFrom(command.FullName);
        if (convertGuardianFullName.Failed) return Result.Failure<CreateAccountResult>(convertGuardianFullName.Error);
        var guardianFullName = convertGuardianFullName.Value;

        var createGuardianAccount = await accountManager.CreateAccountAsync(email, password, cancellationToken);
        if (createGuardianAccount.Failed) return Result.Failure<CreateAccountResult>(createGuardianAccount.Error);
        var guardianAccountInfo = createGuardianAccount.Value;

        var guardian = new Guardian(AccountId.From(guardianAccountInfo.Id), guardianFullName, guardianFullName.Value);
        await guardianRepository.AddAsync(guardian, cancellationToken);

        var issueAuthenticationTokens = await authenticationService.IssueAuthenticationTokens(guardianAccountInfo.Id);
        if (issueAuthenticationTokens.Failed)
            return Result.Failure<CreateAccountResult>(issueAuthenticationTokens.Error);
        var authenticationTokens = issueAuthenticationTokens.Value;

        return Result.Success(new CreateAccountResult
        {
            AccountId = guardianAccountInfo.Id,
            IsEmailConfirmed = guardianAccountInfo.IsEmailConfirmed,
            AccessToken = authenticationTokens.AccessToken,
            ExpiresAt = authenticationTokens.ExpiresAt,
            RefreshToken = authenticationTokens.RefreshToken
        });
    }
}