using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTripApi.Domain.Guardian.GuardianAggregate;
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
        builder.OwnsOne(g => g.Address, address =>
        {
            address.Property(a => a.Street).HasMaxLength(Street.MaxLength);
            address.Property(a => a.StreetNumber).HasMaxLength(StreetNumber.MaxLength);
            address.Property(a => a.Neighborhood).HasMaxLength(Neighborhood.MaxLength);
            address.Property(a => a.City).HasMaxLength(City.MaxLength);
            address.Property(a => a.State).HasMaxLength(State.MaxLength);
            address.Property(a => a.Country).HasMaxLength(Country.MaxLength);
            address.Property(a => a.PostalCode).HasMaxLength(PostalCode.MaxLength);
        });

        builder.OwnsOne(g => g.EmergencyContact, emergencyContact =>
        {
            emergencyContact.Property(ec => ec.Name).HasMaxLength(ContactName.MaxLength);
            emergencyContact.Property(ec => ec.PhoneNumber).HasMaxLength(PhoneNumber.MaxLength);
        });
    }
}