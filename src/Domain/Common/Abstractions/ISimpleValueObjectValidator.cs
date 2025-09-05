using SchoolTripApi.Domain.Common.Exceptions;

namespace SchoolTripApi.Domain.Common.Abstractions;

/// <summary>
///     Contract for simple value objects that require input validation.
/// </summary>
/// <typeparam name="TValue">
///     Type of the <c>Value</c> property in <see cref="SimpleValueObject{TValueObject,TValue}" />.
/// </typeparam>
public interface ISimpleValueObjectValidator<TValue>
{
    /// <summary>
    ///     Validates a simple value object's input value. The method must return the validated value after successful
    ///     validation, or otherwise throw a
    ///     <see cref="ValueObjectException" />.
    /// </summary>
    /// <param name="value">Value to be validated.</param>
    /// <returns></returns>
    static abstract TValue Validate(TValue value);
}