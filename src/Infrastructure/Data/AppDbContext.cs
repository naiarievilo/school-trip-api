using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SchoolTripApi.Domain.AgreementAggregate;
using SchoolTripApi.Domain.EnrollmentAggregate;
using SchoolTripApi.Domain.GradeLevelAggregate;
using SchoolTripApi.Domain.GradeLevelAggregate.ValueObjects;
using SchoolTripApi.Domain.GuardianAggregate;
using SchoolTripApi.Domain.PaymentAggregate;
using SchoolTripApi.Domain.RatingAggregate;
using SchoolTripApi.Domain.SchoolAggregate;
using SchoolTripApi.Domain.SchoolTripAggregate;
using SchoolTripApi.Domain.StudentAggregate;
using SchoolTripApi.Infrastructure.Security.Entities;

namespace SchoolTripApi.Infrastructure.Data;

internal sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : IdentityDbContext<Account, IdentityRole<Guid>, Guid>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<Guardian> Guardians { get; set; }
    public DbSet<Student> Students { get; set; }
    public DbSet<Agreement> Agreements { get; set; }
    public DbSet<AgreementTemplate> AgreementTemplates { get; set; }
    public DbSet<School> Schools { get; set; }
    public DbSet<SchoolTrip> SchoolTrips { get; set; }
    public DbSet<Enrollment> Enrollments { get; set; }
    public DbSet<Rating> Ratings { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<GradeLevel> GradeLevels { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        modelBuilder.Entity<GradeLevel>().HasData(BrazilianGradeLevels.GetBrazilianGradeLevelCodes()
            .Select((gradeLevel, index) => GradeLevel.Create(GradeLevelId.Create(index), gradeLevel))
            .ToList());

        base.OnModelCreating(modelBuilder);
    }
}