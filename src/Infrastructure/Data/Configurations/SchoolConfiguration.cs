using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTripApi.Domain.SchoolAggregate;

namespace SchoolTripApi.Infrastructure.Data.Configurations;

public class SchoolConfiguration : IEntityTypeConfiguration<School>
{
    public void Configure(EntityTypeBuilder<School> builder)
    {
        throw new NotImplementedException();
    }
}