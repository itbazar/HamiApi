using Domain.Models.ComplaintAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ComplaintConfigurations : IEntityTypeConfiguration<Complaint>
{
    public void Configure(EntityTypeBuilder<Complaint> builder)
    {
        builder.HasMany(c => c.ContentsCitizen);
        builder.HasMany(c => c.ContentsInspector);
        builder.OwnsOne(c => c.Password);
    }
}
