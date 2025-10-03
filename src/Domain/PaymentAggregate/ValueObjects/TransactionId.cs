using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.PaymentAggregate.ValueObjects;

public class TransactionId : SimpleValueObject<TransactionId, string>, ISimpleValueObjectValidator<string>
{
    internal TransactionId(string value) : base(Validate(value))
    {
    }

    public static string Validate(string? value)
    {
        return !string.IsNullOrEmpty(value) ? value : throw new ValueObjectException("Transaction ID is required.");
    }
}