using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTripApi.Domain.SchoolGradeAggregate;

namespace SchoolTripApi.Infrastructure.Data.Configurations;

public class SchoolGradeConfiguration : IEntityTypeConfiguration<SchoolGrade>
{
    public void Configure(EntityTypeBuilder<SchoolGrade> builder)
    {
        throw new NotImplementedException();
    }
}