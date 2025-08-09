using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;

namespace SchoolTripApi.Infrastructure.Data.Converters;

public class AccountIdConverter()
    : ValueConverter<AccountId, Guid>(accountId => accountId.Value, guid => AccountId.From(guid));