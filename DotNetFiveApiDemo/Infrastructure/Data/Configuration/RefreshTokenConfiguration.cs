using DotNetFiveApiDemo.Core.Security.Entities;
using DotNetFiveApiDemo.Core.User.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotNetFiveApiDemo.Infrastructure.Data.Configuration
{
    internal sealed class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken<AppUser>>
    {
        public void Configure(EntityTypeBuilder<RefreshToken<AppUser>> builder)
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Id).HasMaxLength(36);
            builder.Property(r => r.Token).HasMaxLength(64);
            builder.HasIndex(r => r.Token).IsUnique();
            builder.Property(r => r.TokenFamily).HasMaxLength(36);

            builder.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId);
        }
    }
}