using Mediator;
using SchoolTripApi.Application.Accounts.Abstractions;
using SchoolTripApi.Application.Accounts.DTOs;
using SchoolTripApi.Application.Common.DTOs;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Accounts.Queries.GetAccountsInfo;

public sealed class GetAccountsInfoHandler(IAccountManager accountManager)
    : IQueryHandler<GetAccountsInfoQuery, Result<PageOf<AccountDto>>>
{
    public async ValueTask<Result<PageOf<AccountDto>>> Handle(GetAccountsInfoQuery query,
        CancellationToken cancellationToken)
    {
        return await accountManager.GetAccountsInfoAsync(query.PaginationDetails, cancellationToken);
    }
}