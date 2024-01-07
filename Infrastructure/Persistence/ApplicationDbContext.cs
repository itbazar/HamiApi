using Domain.Models.ChartAggregate;
using Domain.Models.Common;
using Domain.Models.ComplaintAggregate;
using Domain.Models.IdentityAggregate;
using Domain.Models.PublicKeys;
using Domain.Models.Sliders;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        base.OnModelCreating(builder);
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
}
