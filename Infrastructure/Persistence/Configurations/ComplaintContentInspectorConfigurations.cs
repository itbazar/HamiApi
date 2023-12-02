using Domain.Models.ComplaintAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ComplaintContentInspectorConfiguration : IEntityTypeConfiguration<ComplaintContentInspector>
{
    public void Configure(EntityTypeBuilder<ComplaintContentInspector> builder)
    {
        builder.HasMany(c => c.MediaInspector);
    }
}
