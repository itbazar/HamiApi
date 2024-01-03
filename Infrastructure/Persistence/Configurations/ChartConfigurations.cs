using Domain.Models.ChartAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class ChartConfigurations : IEntityTypeConfiguration<Chart>
{
    public void Configure(EntityTypeBuilder<Chart> builder)
    {
        builder.HasMany(c => c.Users).WithMany();
        builder.HasMany(c => c.Roles).WithMany();
    }
}
