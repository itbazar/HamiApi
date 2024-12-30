using Domain.Models.ChartAggregate;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Domain.Models.DiseaseAggregate;
using Domain.Models.IdentityAggregate;
using Domain.Models.News;
using Domain.Models.PublicKeys;
using Domain.Models.Sliders;
using Domain.Models.WebContents;
using Domain.Primitives;
using MediatR;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

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
}
