using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

public sealed class AccountId : GuidId<AccountId>
{
    private AccountId(Guid value) : base(value)
    {
    }
}