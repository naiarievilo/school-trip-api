using SchoolTripApi.Domain.Common.Abstractions;

namespace SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

public class AccountId(Guid value) : GuidId<AccountId>(value)
{
    protected override AccountId CreateInstance(Guid value)
    {
        return new AccountId(value);
    }
}