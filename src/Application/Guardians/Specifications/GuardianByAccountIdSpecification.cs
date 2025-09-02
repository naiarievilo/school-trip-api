using Ardalis.Specification;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Application.Guardians.Specifications;

public sealed class GuardianByAccountIdSpecification : SingleResultSpecification<Guardian>
{
    public GuardianByAccountIdSpecification(AccountId accountId)
    {
        Query
            .Where(g => g.AccountId.Equals(accountId));
    }
}