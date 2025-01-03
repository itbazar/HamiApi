using Domain.Models.Hami;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class UserGroupMembershipConfigurations : IEntityTypeConfiguration<UserGroupMembership>
{
    public void Configure(EntityTypeBuilder<UserGroupMembership> builder)
    {
        builder.HasOne(ugm => ugm.User).WithMany().HasForeignKey(ugm => ugm.UserId);
        builder.HasOne(ugm => ugm.PatientGroup).WithMany(pg => pg.Members).HasForeignKey(ugm => ugm.PatientGroupId);
    }
}
