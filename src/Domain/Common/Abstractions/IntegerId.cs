using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.Abstractions;

public abstract class IntegerId<TId>(int value) : SimpleValueObject<TId, int>(value)
{
    // For EF Core (integer-based ID autogeneration when using 'SaveChanges')
    protected IntegerId() : this(0)
    {
    }

    public static Result<TId> TryFrom(string? value)
    {
        return TryFrom(value, ConvertToInt);
    }

    public static TId From(string? value)
    {
        return From(value, ConvertToInt);
    }

    private static int ConvertToInt(string? value)
    {
        return int.TryParse(value, out var parsedInt)
            ? parsedInt
            : throw new ValueObjectException(FailedToConvertToValueObject(typeof(TId)));
    }
}