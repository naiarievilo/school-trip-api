using Mediator;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Queries;

public class GetAccountInfoQuery : IQuery<Result<AccountDto>>
{
    private GetAccountInfoQuery(string accountId)
    {
        AccountId = accountId;
    }

    public string AccountId { get; private set; }

    public static GetAccountInfoQuery For(string accountId)
    {
        return new GetAccountInfoQuery(accountId);
    }
}