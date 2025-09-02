using SchoolTripApi.Domain.Common.DTOs;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.Abstractions;

public abstract class GuidId<TId>(Guid value) : StronglyTypedId<TId, Guid>(value)
{
    private const string FailedToConvertToGuid = "'string' provided couldn't be converted to 'Guid'.";

    private static readonly string FailedToConvertToGuidIdSubtype =
        $"Guid provided couldn't be converted to '{typeof(TId)}' value object.";

    public static Result<TId> TryFrom(string? value)
    {
        return TryFrom(value, TryConvertToGuid);
    }

    public static TId From(Guid value)
    {
        return (TId)Activator.CreateInstance(typeof(TId), value)!;
    }

    protected TId ConvertToId(string? value)
    {
        return Guid.TryParse(value, out var parsedGuid)
            ? From(parsedGuid)
            : throw new ValueObjectException(FailedToConvertToGuidIdSubtype);
    }

    private static Result<Guid> TryConvertToGuid(string? value)
    {
        return Guid.TryParse(value, out var parsedGuid)
            ? Result.Success(parsedGuid)
            : throw new ValueObjectException(FailedToConvertToGuid);
    }

    protected static Guid ConvertToGuid(string? value)
    {
        return Guid.TryParse(value, out var parsedGuid)
            ? parsedGuid
            : throw new ValueObjectException(FailedToConvertToGuid);
    }
}