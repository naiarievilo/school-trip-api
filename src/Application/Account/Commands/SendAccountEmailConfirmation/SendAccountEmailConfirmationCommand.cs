using Mediator;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Commands.SendAccountEmailConfirmation;

public class SendAccountEmailConfirmationCommand : ICommand<Result>
{
    private SendAccountEmailConfirmationCommand(string accountId)
    {
        AccountId = accountId;
    }

    public string AccountId { get; private set; }

    public static SendAccountEmailConfirmationCommand For(string accountId)
    {
        return new SendAccountEmailConfirmationCommand(accountId);
    }
}