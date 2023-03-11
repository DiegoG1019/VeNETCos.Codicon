using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using VeNETCos.Codicon.Database.Models;

namespace VeNETCos.Codicon.Database.Contexts;
public class AppDbContext : DbContext
{
    public DbSet<FileLink> FileLinks => Set<FileLink>();
    public DbSet<Box> Boxes => Set<Box>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
#if DEBUG
        //Database.EnsureDeleted();
#endif
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);
        ConfigureAppModel(mb.Entity<FileLink>());
        ConfigureBoxModel(mb.Entity<Box>());

        SeedDatabase(mb);
    }

    private static void ConfigureBoxModel(EntityTypeBuilder<Box> mb)
    {
        mb.HasKey(x => x.Id);
        mb.HasMany(x => x.Apps).WithMany(x => x.Boxes);
        mb.HasOne(x => x.Parent).WithMany(x => x.Children);
        mb.Navigation(x => x.Apps).AutoInclude();
    }

    private static void ConfigureAppModel(EntityTypeBuilder<FileLink> mb)
    {
        mb.HasKey(x => x.Id);
    }

    private static void SeedDatabase(ModelBuilder mb)
    {

    }
}
