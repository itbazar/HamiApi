using Domain.Models.Hami;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configurations;

public class AnswerConfigurations : IEntityTypeConfiguration<Answer>
{
    public void Configure(EntityTypeBuilder<Answer> builder)
    {
        builder.HasOne(a => a.Question).WithMany().HasForeignKey(a => a.QuestionId);
        builder.HasOne(a => a.User).WithMany().HasForeignKey(a => a.UserId);
        builder.HasOne(a => a.TestPeriod).WithMany().HasForeignKey(a => a.TestPeriodId);
    }
}
