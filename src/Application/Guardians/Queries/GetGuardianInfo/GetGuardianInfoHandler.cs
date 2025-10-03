using Mediator;
using SchoolTripApi.Application.Common.Abstractions;
using SchoolTripApi.Application.Guardians.DTOs;
using SchoolTripApi.Application.Guardians.Specifications;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Application.Guardians.Queries.GetGuardianInfo;

public sealed class GetGuardianInfoHandler(IRepository<Guardian> guardianRepository)
    : IQueryHandler<GetGuardianInfoQuery, Result<GuardianDto>>
{
    public async ValueTask<Result<GuardianDto>> Handle(GetGuardianInfoQuery query, CancellationToken cancellationToken)
    {
        var convertToAccountId = AccountId.TryFrom(query.AccountId);
        if (convertToAccountId.Failed) return Result.Failure<GuardianDto>(convertToAccountId.Error);
        var accountId = convertToAccountId.Value;

        var spec = new GuardianByAccountIdSpecification(accountId);
        var guardian = await guardianRepository.FirstOrDefaultAsync(spec, cancellationToken);
        if (guardian is null) return Result.Failure<GuardianDto>(GuardianError.GuardianNotFound(accountId.Value));

        return Result.Success(new GuardianDto
        {
            FullName = guardian.FullName,
            Cpf = guardian.Cpf,
            Address = guardian.Address,
            EmergencyContact = guardian.EmergencyContact
        });
    }
}