using Domain.Models.Hami;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserMedicalInfoConfigurations : IEntityTypeConfiguration<UserMedicalInfo>
{
    public void Configure(EntityTypeBuilder<UserMedicalInfo> builder)
    {
        builder.HasMany(umi => umi.MedicalEntries).WithOne(me => me.UserMedicalInfo).HasForeignKey(me => me.UserMedicalInfoId);
    }
}
