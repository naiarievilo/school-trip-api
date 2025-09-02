using Mediator;
using SchoolTripApi.Application.Guardians.DTOs;
using SchoolTripApi.Domain.Common.DTOs;

namespace SchoolTripApi.Application.Guardians.Queries.GetGuardianInfo;

public sealed class GetGuardianInfoQuery : IQuery<Result<GuardianDto>>
{
    private GetGuardianInfoQuery(string accountId)
    {
        AccountId = accountId;
    }

    public string AccountId { get; private set; }

    public static GetGuardianInfoQuery For(string accountId)
    {
        return new GetGuardianInfoQuery(accountId);
    }
}