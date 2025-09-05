using AutoMapper;
using Mediator;
using SchoolTripApi.Application.Common.Abstractions;
using SchoolTripApi.Application.Guardians.Errors;
using SchoolTripApi.Application.Guardians.Specifications;
using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Application.Guardians.Commands.UpdateGuardianInfo;

public sealed class UpdateGuardianInfoHandler(IRepository<Guardian> guardianRepository, IMapper mapper)
    : ICommandHandler<UpdateGuardianInfoCommand, Result>
{
    public async ValueTask<Result> Handle(UpdateGuardianInfoCommand command, CancellationToken cancellationToken)
    {
        var convertToAccountId = AccountId.TryFrom(command.AccountId);
        if (convertToAccountId.Failed) return Result.Failure(convertToAccountId.Error);
        var accountId = convertToAccountId.Value;

        var specification = new GuardianByAccountIdSpecification(accountId);
        var guardian = await guardianRepository.FirstOrDefaultAsync(specification, cancellationToken);
        if (guardian is null) return Result.Failure(GuardianError.GuardianNotFound(accountId.Value));

        mapper.Map(command, guardian);
        guardian.UpdateLastModified(guardian.FullName?.Value);
        await guardianRepository.UpdateAsync(guardian, cancellationToken);

        return Result.Success();
    }
}