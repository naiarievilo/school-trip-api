using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace SchoolTripApi.Domain.Common.Abstractions;

/// <summary>
///     Handles constructor compilation for subtypes of <see cref="SimpleValueObject{TValueObject,TValue}" />> for better
///     performance compared to using <see cref="Activator" />.
/// </summary>
public abstract class SimpleValueObjectConstructorFactory
{
    private static readonly ConcurrentDictionary<Type, object> ConstructorCache = new();

    public static Func<TValue, TValueObject> GetValueObjectConstructor<TValueObject, TValue>()
    {
        return (Func<TValue, TValueObject>)ConstructorCache.GetOrAdd(typeof(TValueObject),
            _ => CreateValueObjectConstructorDelegate<TValueObject, TValue>());
    }

    private static Func<TValue, TValueObject> CreateValueObjectConstructorDelegate<TValueObject, TValue>()
    {
        var valueObject = typeof(TValueObject);

        var constructor = valueObject.GetConstructor(BindingFlags.Instance | BindingFlags.NonPublic, [typeof(TValue)]);
        if (constructor is null || constructor.GetParameters().Length > 1)
            throw new InvalidOperationException(
                $"Value object '{valueObject.Name}' must have a constructor that accepts one '{typeof(TValue).Name}' parameter.");

        var parameter = Expression.Parameter(typeof(TValue), constructor.GetParameters()[0].Name);
        var constructorCall = Expression.New(constructor, parameter);

        var lambda = Expression.Lambda<Func<TValue, TValueObject>>(constructorCall, parameter);
        return lambda.Compile();
    }
}