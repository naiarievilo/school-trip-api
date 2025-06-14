using Mediator;
using SchoolTripApi.Application.Common.Security.Abstractions;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Account.Queries;

public class GetAccountInfoHandler(IAccountManager accountManager)
    : IQueryHandler<GetAccountInfoQuery, Result<AccountDto>>
{
    public async ValueTask<Result<AccountDto>> Handle(GetAccountInfoQuery query, CancellationToken cancellationToken)
    {
        var getAccountInfo = await accountManager.GetAccountInfoAsync(query.AccountId, cancellationToken);
        return getAccountInfo.Succeeded
            ? Result.Success(getAccountInfo.Value)
            : Result.Failure<AccountDto>(getAccountInfo.Error);
    }
}