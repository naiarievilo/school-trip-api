using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTripApi.Domain.Guardian.GuardianAggregate;
using SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects;
using SchoolTripApi.Infrastructure.Data.Converters;
using City = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.City;
using ContactName = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.ContactName;
using Country = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.Country;
using Neighborhood = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.Neighborhood;
using PhoneNumber = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.PhoneNumber;
using PostalCode = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.PostalCode;
using State = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.State;
using Street = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.Street;
using StreetNumber = SchoolTripApi.Domain.Guardian.GuardianAggregate.ValueObjects.StreetNumber;

namespace SchoolTripApi.Infrastructure.Data.Configurations;

public class GuardianConfiguration : IEntityTypeConfiguration<Guardian>
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