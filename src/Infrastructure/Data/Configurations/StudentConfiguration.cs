using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTripApi.Domain.Common.ValueObjects;
using SchoolTripApi.Domain.StudentAggregate;

namespace SchoolTripApi.Infrastructure.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasOne(s => s.Guardian)
            .WithMany(g => g.Students)
            .HasForeignKey(s => s.GuardianId)
            .IsRequired();

        builder.HasOne(s => s.School)
            .WithMany(g => g.Students)
            .HasForeignKey(s => s.SchoolId)
            .IsRequired();

        builder.HasOne(s => s.SchoolGrade)
            .WithMany(sg => sg.Students)
            .HasForeignKey(s => s.SchoolGradeId)
            .IsRequired();

        builder.Property(s => s.FullName).HasMaxLength(FullName.MaxLength);
        builder.Property(s => s.Cpf).HasMaxLength(Cpf.MaxLength);
    }
}