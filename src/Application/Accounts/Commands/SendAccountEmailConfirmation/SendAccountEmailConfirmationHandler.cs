using Mediator;
using SchoolTripApi.Application.Accounts.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Application.Accounts.Commands.SendAccountEmailConfirmation;

public sealed class SendAccountEmailConfirmationHandler(
    IAccountManager accountManager,
    ISecurityTokenProvider securityTokenProvider,
    ISecurityEmailService securityEmailService) : ICommandHandler<SendAccountEmailConfirmationCommand, Result>
{
    public async ValueTask<Result> Handle(SendAccountEmailConfirmationCommand command,
        CancellationToken cancellationToken)
    {
        var convertToAccountId = AccountId.TryFrom(command.AccountId);
        if (convertToAccountId.Failed) return Result.Failure(convertToAccountId.Error);
        var accountId = convertToAccountId.Value;

        var getAccountInfo = await accountManager.GetAccountInfoAsync(accountId, cancellationToken);
        if (getAccountInfo.Failed) return Result.Failure(getAccountInfo.Error);
        var accountInfo = getAccountInfo.Value;

        var generateEmailConfirmationToken =
            await securityTokenProvider.GenerateEmailConfirmationTokenAsync(accountInfo.Email);
        if (generateEmailConfirmationToken.Failed) return Result.Failure(generateEmailConfirmationToken.Error);
        var emailConfirmationToken = generateEmailConfirmationToken.Value;

        return await securityEmailService.SendEmailConfirmationLinkAsync(accountInfo.Email, emailConfirmationToken);
    }
}