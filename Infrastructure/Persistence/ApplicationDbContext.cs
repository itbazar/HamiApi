using Domain.Models.ChartAggregate;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Domain.Models.Hami;
using Domain.Models.IdentityAggregate;
using Domain.Models.News;
using Domain.Models.PublicKeys;
using Domain.Models.Sliders;
using Domain.Models.WebContents;
using Domain.Primitives;
using Infrastructure.Persistence.Data.Seed;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    private readonly IPublisher _publisher;
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher)
        : base(options)
    {
        _publisher = publisher;
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(builder);

       QuestionSeeder.SeedQuestions(builder);

        builder.Entity<TestPeriod>().HasData(
        new
        {
            Id = new Guid("11111111-1111-1111-1111-111111111111"),
            TestType = TestType.GAD,
            PeriodName = "ارزیابی اولیه GAD هنگام ثبت نام",
            StartDate = new DateTime(2025, 1, 1),
            EndDate = new DateTime(2099, 12, 31),
            Code = 101, // کد یکتا برای GAD
            IsDeleted = false,
            Recurrence = RecurrenceType.None // مقدار پیش‌فرض
        },
        new
        {
            Id = new Guid("22222222-2222-2222-2222-222222222222"),
            TestType = TestType.MDD,
            PeriodName = "ارزیابی اولیه MDD هنگام ثبت نام",
            StartDate = new DateTime(2025, 1, 1),
            EndDate = new DateTime(2099, 12, 31),
            Code = 102, // کد یکتا برای MDD
            IsDeleted = false,
            Recurrence = RecurrenceType.None // مقدار پیش‌فرض
        }
    );
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        var domainEvents = ChangeTracker.Entries<Entity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .SelectMany(e => e.DomainEvents)
            .ToList();

        var result = await base.SaveChangesAsync(cancellationToken);
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }
        return result;
    }
    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.LogTo(Console.WriteLine);
    public DbSet<Complaint> Complaint { get; set; }
    public DbSet<ComplaintCategory> ComplaintCategory { get; set; }
    public DbSet<ComplaintOrganization> ComplaintOrganization { get; set; }
    public DbSet<Media> Media { get; set; }
    public DbSet<Chart> Chart { get; set; }
    public DbSet<PublicKey> PublicKey { get; set; }
    public DbSet<Slider> Slider { get; set; }
    public DbSet<WebContent> WebContent { get; set; }
    public DbSet<News> News { get; set; }
    public DbSet<Disease> Diseases { get; set; }
    public DbSet<Stage> Stages { get; set; }
    public DbSet<TestPeriod> TestPeriod { get; set; }
    public DbSet<Question> Question { get; set; }
    public DbSet<Answer> Answer { get; set; }
    public DbSet<TestPeriodResult> TestPeriodResult { get; set; }
    public DbSet<UserMedicalInfo> UserMedicalInfo { get; set; }
    public DbSet<UserGroupMembership> UserGroupMembership { get; set; }
    public DbSet<CounselingSession> CounselingSession { get; set; }
    public DbSet<PatientGroup> PatientGroup { get; set; }
    public DbSet<SessionAttendanceLog> SessionAttendanceLog { get; set; }

}
