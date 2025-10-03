using SchoolTripApi.Domain.Common.Errors;

namespace SchoolTripApi.Domain.GuardianAggregate;

public sealed class GuardianError(string code, string description) : Error(code, description)
{
    private const string GuardianNotFoundCode = "GuardianError.GuardianNotFound";
    private const string FailedToAssignGuardianshipCode = "GuardianError.FailedToAssignGuardianship";
    private const string FailedToRevokeGuardianshipCode = "GuardianError.FailedToRevokeGuardianship";

    public static Error GuardianNotFound(Guid accountId)
    {
        return new GuardianError(GuardianNotFoundCode, $"Guardian with account ID '{accountId}' not found.");
    }

    public static Error FailedToAssignGuardianship()
    {
        return new GuardianError(FailedToAssignGuardianshipCode, "Student is already assigned to another guardian.");
    }

    public static Error FailedToRevokeGuardianship()
    {
        return new GuardianError(FailedToRevokeGuardianshipCode,
            "Guardian is not assigned to the student's guardianship.");
    }
}