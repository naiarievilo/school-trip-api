namespace SchoolTripApi.Domain.Common.Errors;

public sealed class ValueObjectError(string code, string description) : Error(code, description)
{
    public const string FailedToConvertToValueObjectCode = "ValueObjectError.FailedToConvertToValueObject";

    public static Error FailedToConvertToValueObject(string message)
    {
        return new ValueObjectError(FailedToConvertToValueObjectCode, message);
    }
}