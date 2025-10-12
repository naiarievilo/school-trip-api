using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTripApi.Domain.SchoolTripAggregate;

namespace SchoolTripApi.Infrastructure.Data.Configurations.Entities;

internal class SchoolTripConfiguration : IEntityTypeConfiguration<SchoolTrip>
{
    public void Configure(EntityTypeBuilder<SchoolTrip> builder)
    {
        builder.HasMany(t => t.Agreements)
            .WithOne(ag => ag.SchoolTrip)
            .HasForeignKey(ag => ag.SchoolTripId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Enrollments)
            .WithOne(e => e.SchoolTrip)
            .HasForeignKey(e => e.SchoolTripId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Ratings)
            .WithOne(r => r.SchoolTrip)
            .HasForeignKey(r => r.SchoolTripId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Grades)
            .WithMany(sg => sg.Trips);

        builder.OwnsOne(st => st.ParticipantsCapacity, ownedBuilder => ownedBuilder.ConfigureValueObjects());
        builder.OwnsOne(st => st.Price, ownedBuilder => ownedBuilder.ConfigureValueObjects());
        builder.OwnsOne(st => st.DepartureAddress, ownedBuilder => ownedBuilder.ConfigureValueObjects());
        builder.ConfigureValueObjects();
    }
}