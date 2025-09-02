using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.GuardianAggregate.ValueObjects;
using SchoolTripApi.Infrastructure.Data.Converters;
using City = SchoolTripApi.Domain.Common.ValueObjects.City;
using ContactName = SchoolTripApi.Domain.GuardianAggregate.ValueObjects.ContactName;
using Country = SchoolTripApi.Domain.Common.ValueObjects.Country;
using Neighborhood = SchoolTripApi.Domain.Common.ValueObjects.Neighborhood;
using PhoneNumber = SchoolTripApi.Domain.GuardianAggregate.ValueObjects.PhoneNumber;
using PostalCode = SchoolTripApi.Domain.Common.ValueObjects.PostalCode;
using State = SchoolTripApi.Domain.Common.ValueObjects.State;
using Street = SchoolTripApi.Domain.Common.ValueObjects.Street;
using StreetNumber = SchoolTripApi.Domain.Common.ValueObjects.StreetNumber;

namespace SchoolTripApi.Infrastructure.Data.Configurations;

internal sealed class GuardianConfiguration : IEntityTypeConfiguration<Guardian>
{
    public void Configure(EntityTypeBuilder<Guardian> builder)
    {
        builder.Property(g => g.Id).HasConversion<GuardianIdConverter>();
        builder.Property(g => g.AccountId).HasConversion<AccountIdConverter>();

        builder.Property(g => g.FullName)
            // Ignore the null reference warning since the full name is nullable
            .HasConversion(fullName => fullName!.Value, str => FullName.From(str))
            .HasMaxLength(FullName.MaxLength);

        builder.Property(g => g.Cpf)
            // Ignore the null reference warning since CPF is nullable
            .HasConversion(cpf => cpf!.Value, str => Cpf.From(str))
            .HasMaxLength(Cpf.MaxLength);

        builder.OwnsOne(g => g.Address, address =>
        {
            address.Property(a => a.Street)
                .HasConversion(street => street.Value, str => Street.From(str))
                .HasMaxLength(Street.MaxLength);
            address.Property(a => a.StreetNumber)
                .HasConversion(streetNumber => streetNumber.Value, str => StreetNumber.From(str))
                .HasMaxLength(StreetNumber.MaxLength);
            address.Property(a => a.Neighborhood)
                .HasConversion(neighborhood => neighborhood.Value, str => Neighborhood.From(str))
                .HasMaxLength(Neighborhood.MaxLength);
            address.Property(a => a.City)
                .HasConversion(city => city.Value, str => City.From(str))
                .HasMaxLength(City.MaxLength);
            address.Property(a => a.State)
                .HasConversion(state => state.Value, str => State.From(str))
                .HasMaxLength(State.MaxLength);
            address.Property(a => a.Country)
                .HasConversion(country => country.Value, str => Country.From(str))
                .HasMaxLength(Country.MaxLength);
            address.Property(a => a.PostalCode)
                .HasConversion(postalCode => postalCode.Value, str => PostalCode.From(str))
                .HasMaxLength(PostalCode.MaxLength);
        });

        builder.OwnsOne(g => g.EmergencyContact, emergencyContact =>
        {
            emergencyContact.Property(ec => ec.Name)
                .HasConversion(contactName => contactName.Value, str => ContactName.From(str))
                .HasMaxLength(ContactName.MaxLength);
            emergencyContact.Property(ec => ec.PhoneNumber)
                .HasConversion(phoneNumber => phoneNumber.Value, str => PhoneNumber.From(str))
                .HasMaxLength(PhoneNumber.MaxLength);
        });
    }
}