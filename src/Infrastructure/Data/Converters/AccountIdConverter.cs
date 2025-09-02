using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Infrastructure.Data.Converters;

internal sealed class AccountIdConverter()
    : ValueConverter<AccountId, Guid>(accountId => accountId.Value, guid => AccountId.From(guid));