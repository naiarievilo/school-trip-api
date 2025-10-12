using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SchoolTripApi.Domain.AgreementAggregate;

namespace SchoolTripApi.Infrastructure.Data.Configurations.Entities;

public class AgreementTemplateConfiguration : IEntityTypeConfiguration<AgreementTemplate>
{
    public void Configure(EntityTypeBuilder<AgreementTemplate> builder)
    {
        builder.HasMany(at => at.Agreements)
            .WithOne(ag => ag.AgreementTemplate)
            .HasForeignKey(ag => ag.AgreementTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(at => at.SchoolTrips)
            .WithOne(st => st.AgreementTemplate)
            .OnDelete(DeleteBehavior.Restrict);

        builder.ConfigureValueObjects();
    }
}