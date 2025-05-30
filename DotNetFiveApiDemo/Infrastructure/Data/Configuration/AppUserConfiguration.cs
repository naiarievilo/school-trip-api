using DotNetFiveApiDemo.Core.User.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetFiveApiDemo.Infrastructure.Data.Configuration
{
    internal sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public const int FirstNameMaxLength = 64;
        public const int LastNameMaxLength = 64;
        public const int StreetMaxLength = 256;
        public const int NumberMaxLength = 32;
        public const int CityMaxLength = 64;
        public const int StateMaxLength = 64;
        public const int CountryMaxLength = 64;
        public const int PostalCodeMaxLength = 32;

        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(u => u.FirstName).HasMaxLength(FirstNameMaxLength).IsRequired();
            builder.Property(u => u.LastName).HasMaxLength(LastNameMaxLength).IsRequired();

            builder.OwnsOne(u => u.Address, address =>
            {
                address.Property(a => a.Street).HasColumnName("Street").HasMaxLength(StreetMaxLength);
                address.Property(a => a.Number).HasColumnName("Number").HasMaxLength(NumberMaxLength);
                address.Property(a => a.City).HasColumnName("City").HasMaxLength(CityMaxLength);
                address.Property(a => a.State).HasColumnName("State").HasMaxLength(StateMaxLength);
                address.Property(a => a.Country).HasColumnName("Country").HasMaxLength(CountryMaxLength);
                address.Property(a => a.PostalCode).HasColumnName("PostalCode").HasMaxLength(PostalCodeMaxLength);
            });
        }
    }
}