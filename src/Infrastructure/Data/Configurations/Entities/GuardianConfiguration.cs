using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTripApi.Domain.GuardianAggregate;

namespace SchoolTripApi.Infrastructure.Data.Configurations.Entities;

internal sealed class GuardianConfiguration : IEntityTypeConfiguration<Guardian>
{
    public void Configure(EntityTypeBuilder<Guardian> builder)
    {
        builder.HasMany(g => g.Agreements)
            .WithOne(a => a.Guardian)
            .HasForeignKey(a => a.GuardianId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Enrollments)
            .WithOne(e => e.Guardian)
            .HasForeignKey(e => e.GuardianId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Payments)
            .WithOne(p => p.Guardian)
            .HasForeignKey(p => p.GuardianId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Ratings)
            .WithOne(r => r.Guardian)
            .HasForeignKey(r => r.GuardianId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(g => g.Students)
            .WithOne(s => s.Guardian)
            .HasForeignKey(s => s.GuardianId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(g => g.Cpf).IsUnique();

        builder.OwnsOne(g => g.Address, ownedBuilder => ownedBuilder.ConfigureValueObjects());
        builder.OwnsOne(g => g.EmergencyContact, ownedBuilder => ownedBuilder.ConfigureValueObjects());
        builder.ConfigureValueObjects();
    }
}