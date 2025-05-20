using DotNetFiveApiDemo.Application.User.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace DotNetFiveApiDemo.Infrastructure.Data.Identity
{
    public class AppDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .OwnsOne(u => u.Address, address =>
                {
                    address.Property(a => a.Street).HasColumnName("Street");
                    address.Property(a => a.Number).HasColumnName("Number");
                    address.Property(a => a.City).HasColumnName("City");
                    address.Property(a => a.State).HasColumnName("State");
                    address.Property(a => a.Country).HasColumnName("Country");
                    address.Property(a => a.ZipCode).HasColumnName("ZipCode");
                });
        }
    }
}