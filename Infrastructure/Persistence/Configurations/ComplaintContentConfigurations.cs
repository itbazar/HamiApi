using Domain.Models.ComplaintAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ComplaintContentCitizenConfiguration : IEntityTypeConfiguration<ComplaintContent>
{
    public void Configure(EntityTypeBuilder<ComplaintContent> builder)
    {
        builder.HasMany(c => c.Media);
    }
}
