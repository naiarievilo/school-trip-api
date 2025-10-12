using SchoolTripApi.Domain.Common.Abstractions;
using SchoolTripApi.Domain.Common.Enums;

namespace SchoolTripApi.Domain.Common.ValueObjects;

public sealed class Currency(CurrencyCode value) : SimpleValueObject<Currency, CurrencyCode>(value);