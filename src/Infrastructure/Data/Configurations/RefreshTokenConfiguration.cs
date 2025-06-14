using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTripApi.Infrastructure.Security.Entities;

namespace SchoolTripApi.Infrastructure.Data.Configurations;

internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
{
    public void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder.HasKey(r => r.Id);
        builder.Property(r => r.Id).HasMaxLength(36);
        builder.Property(r => r.Token).HasMaxLength(64);
        builder.HasIndex(r => r.Token).IsUnique();
        builder.Property(r => r.TokenFamily).HasMaxLength(36);

        builder.HasOne(r => r.User).WithMany().HasForeignKey(r => r.AccountId);
    }
}