using Mediator;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Commands.SendPasswordResetCode;

public class SendPasswordResetCodeHandler(
    ISecurityTokenProvider tokenProvider,
    ISecurityEmailService securityEmailService)
    : ICommandHandler<SendPasswordResetCodeCommand, Result>
{
    public async ValueTask<Result> Handle(SendPasswordResetCodeCommand command, CancellationToken cancellationToken)
    {
        var email = command.Email;

        var generatePasswordResetCode = await tokenProvider.GeneratePasswordResetCodeAsync(email);
        if (generatePasswordResetCode.Failed) return Result.Failure(generatePasswordResetCode.Error);
        var passwordResetCode = generatePasswordResetCode.Value;

        var sendPasswordResetCode = await securityEmailService.SendPasswordResetCodeAsync(email, passwordResetCode);
        return sendPasswordResetCode.Succeeded
            ? Result.Success()
            : Result.Failure(sendPasswordResetCode.Error);
    }
}