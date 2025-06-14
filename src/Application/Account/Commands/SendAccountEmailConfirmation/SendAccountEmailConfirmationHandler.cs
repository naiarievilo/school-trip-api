using Mediator;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Commands.SendAccountEmailConfirmation;

public class SendAccountEmailConfirmationHandler(
    IAccountManager accountManager,
    ISecurityTokenProvider securityTokenProvider,
    ISecurityEmailService securityEmailService) : ICommandHandler<SendAccountEmailConfirmationCommand, Result>
{
    public async ValueTask<Result> Handle(SendAccountEmailConfirmationCommand command,
        CancellationToken cancellationToken)
    {
        var getAccountInfo = await accountManager.GetAccountInfoAsync(command.AccountId, cancellationToken);
        if (getAccountInfo.Failed) return Result.Failure(getAccountInfo.Error);
        var accountInfo = getAccountInfo.Value;

        var generateEmailConfirmationToken =
            await securityTokenProvider.GenerateEmailConfirmationTokenAsync(accountInfo.Email);
        if (generateEmailConfirmationToken.Failed) return Result.Failure(generateEmailConfirmationToken.Error);
        var emailConfirmationToken = generateEmailConfirmationToken.Value;

        return await securityEmailService.SendEmailConfirmationLinkAsync(accountInfo.Email, emailConfirmationToken);
    }
}