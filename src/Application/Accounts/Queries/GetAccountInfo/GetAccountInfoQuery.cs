using Mediator;
using SchoolTripApi.Application.Accounts.DTOs;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Accounts.Queries.GetAccountInfo;

public sealed class GetAccountInfoQuery : IQuery<Result<AccountDto>>
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