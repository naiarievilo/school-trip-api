using Mediator;
using SchoolTripApi.Application.Accounts.DTOs;
using SchoolTripApi.Application.Common.DTOs;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Accounts.Queries.GetAccountsInfo;

public sealed class GetAccountsInfoQuery : IQuery<Result<PageOf<AccountDto>>>
{
    private GetAccountsInfoQuery(PaginationDetails paginationDetails)
    {
        PaginationDetails = paginationDetails;
    }

    public PaginationDetails PaginationDetails { get; private set; }

    public static GetAccountsInfoQuery With(PaginationDetails paginationDetails)
    {
        return new GetAccountsInfoQuery(paginationDetails);
    }
}