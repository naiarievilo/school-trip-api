using Mediator;
using SchoolTripApi.Domain.Common.DTOs;
using AccountId = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.AccountId;

namespace SchoolTripApi.Application.Account.Queries;

public class GetAccountInfoQuery : IQuery<Result<AccountDto>>
{
    private GetAccountInfoQuery(AccountId accountId)
    {
        AccountId = accountId;
    }

    public AccountId AccountId { get; private set; }

    public static GetAccountInfoQuery For(AccountId accountId)
    {
        return new GetAccountInfoQuery(accountId);
    }
}