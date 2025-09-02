using SchoolTripApi.Domain.Common.Errors;

namespace SchoolTripApi.Application.Guardians.Errors;

public sealed class GuardianError(string code, string description) : Error(code, description)
{
    private const string GuardianNotFoundCode = "GuardianError.GuardianNotFound";

    public static Error GuardianNotFound(Guid accountId)
    {
        return new GuardianError(GuardianNotFoundCode, $"Guardian with account ID '{accountId}' not found.");
    }
}