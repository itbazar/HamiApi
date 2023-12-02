using Domain.Models.ComplaintAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ComplaintContentCitizenConfiguration : IEntityTypeConfiguration<ComplaintContentCitizen>
{
    public void Configure(EntityTypeBuilder<ComplaintContentCitizen> builder)
    {
        builder.HasMany(c => c.MediaCitizen);
    }
}
