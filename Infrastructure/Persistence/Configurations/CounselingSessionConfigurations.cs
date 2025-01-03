using Domain.Models.Hami;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class CounselingSessionConfigurations : IEntityTypeConfiguration<CounselingSession>
{
    public void Configure(EntityTypeBuilder<CounselingSession> builder)
    {
        builder.HasOne(cs => cs.PatientGroup).WithMany(pg => pg.Sessions).HasForeignKey(cs => cs.PatientGroupId);
        builder.HasOne(cs => cs.Mentor).WithMany().HasForeignKey(cs => cs.MentorId);
        builder.HasMany(cs => cs.AttendanceLogs).WithOne(al => al.CounselingSession).HasForeignKey(al => al.CounselingSessionId);
    }
}
