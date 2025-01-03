using Domain.Models.Hami;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class TestPeriodResultConfigurations : IEntityTypeConfiguration<TestPeriodResult>
{
    public void Configure(EntityTypeBuilder<TestPeriodResult> builder)
    {
        builder.HasOne(r => r.TestPeriod).WithMany().HasForeignKey(r => r.TestPeriodId);
        builder.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId);
    }
}
