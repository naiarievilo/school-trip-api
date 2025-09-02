using Mediator;
using SchoolTripApi.Application.Accounts.Abstractions;
using SchoolTripApi.Application.Accounts.DTOs;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Application.Accounts.Queries.GetAccountInfo;

public sealed class GetAccountInfoHandler(IAccountManager accountManager)
    : IQueryHandler<GetAccountInfoQuery, Result<AccountDto>>
{
    public async ValueTask<Result<AccountDto>> Handle(GetAccountInfoQuery query, CancellationToken cancellationToken)
    {
        var convertToAccountId = AccountId.TryFrom(query.AccountId);
        if (convertToAccountId.Failed) return Result.Failure<AccountDto>(convertToAccountId.Error);
        var accountId = convertToAccountId.Value;

        var getAccountInfo = await accountManager.GetAccountInfoAsync(accountId, cancellationToken);
        return getAccountInfo.Succeeded
            ? Result.Success(getAccountInfo.Value)
            : Result.Failure<AccountDto>(getAccountInfo.Error);
    }
}