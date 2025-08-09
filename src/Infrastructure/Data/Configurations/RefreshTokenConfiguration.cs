using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTripApi.Infrastructure.Security.Entities;

namespace SchoolTripApi.Infrastructure.Data.Configurations;

internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(rt => rt.Id);
        builder.Property(rt => rt.Id).HasMaxLength(36);
        builder.Property(rt => rt.Token).HasMaxLength(64);
        builder.HasIndex(rt => rt.Token).IsUnique();
        builder.Property(rt => rt.TokenFamily).HasMaxLength(36);
        builder.Property(rt => rt.ExpiresAt).HasDefaultValue(DateTime.UtcNow);

        builder.HasOne(rt => rt.User).WithMany().HasForeignKey(rt => rt.AccountId);
    }
}