using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTripApi.Domain.PaymentAggregate;

namespace SchoolTripApi.Infrastructure.Data.Configurations.Entities;

public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
{
    public void Configure(EntityTypeBuilder<Payment> builder)
    {
        builder.HasMany(p => p.Enrollments)
            .WithOne(e => e.Payment)
            .HasForeignKey(e => e.PaymentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.OwnsOne(p => p.Amount, ownedBuilder => ownedBuilder.ConfigureValueObjects());
        builder.ConfigureValueObjects();
    }
}