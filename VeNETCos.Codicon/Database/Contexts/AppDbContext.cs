using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using VeNETCos.Codicon.Database.Models;

namespace VeNETCos.Codicon.Database.Contexts;
public class AppDbContext : DbContext
{
    public DbSet<BoxedApp> BoxedApps => Set<BoxedApp>();
    public DbSet<AppBox> AppBoxes => Set<AppBox>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
#if DEBUG
        Database.EnsureDeleted();
#endif
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);
        ConfigureAppModel(mb.Entity<BoxedApp>());
        ConfigureBoxModel(mb.Entity<AppBox>());

        SeedDatabase(mb);
    }

    private static void ConfigureBoxModel(EntityTypeBuilder<AppBox> mb)
    {
        mb.HasKey(x => x.Id);
        mb.HasMany(x => x.Apps).WithMany(x => x.Boxes);
        mb.HasOne(x => x.Parent).WithMany();
    }

    private static void ConfigureAppModel(EntityTypeBuilder<BoxedApp> mb)
    {
        mb.HasKey(x => x.Id);
        mb.HasMany(x => x.Boxes).WithMany(x => x.Apps);
    }

    private static void SeedDatabase(ModelBuilder mb)
    {

    }
}
