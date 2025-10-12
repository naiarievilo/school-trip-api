using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTripApi.Domain.SchoolAggregate;

namespace SchoolTripApi.Infrastructure.Data.Configurations.Entities;

internal sealed class SchoolConfiguration : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        builder.HasMany(sc => sc.Grades)
            .WithMany(sg => sg.Schools);

        builder.HasMany(sc => sc.Guardians)
            .WithMany(g => g.Schools);

        builder.HasMany(sc => sc.Students)
            .WithOne(st => st.School)
            .HasForeignKey(st => st.SchoolId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(sc => sc.Trips)
            .WithOne(t => t.School)
            .HasForeignKey(t => t.SchoolId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.OwnsOne(s => s.Address, ownedBuilder => ownedBuilder.ConfigureValueObjects());
        builder.ConfigureValueObjects();
    }
}