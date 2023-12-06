using Domain.Models.ComplaintAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ComplaintConfigurations : IEntityTypeConfiguration<Complaint>
{
    public void Configure(EntityTypeBuilder<Complaint> builder)
    {
        builder.HasMany(c => c.Contents);
        
        builder.OwnsOne(c => c.CitizenPassword);
        builder.OwnsOne(c => c.EncryptionKeyPassword);
    }
}
