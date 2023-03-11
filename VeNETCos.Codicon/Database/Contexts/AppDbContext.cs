using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using VeNETCos.Codicon.Database.Models;

namespace VeNETCos.Codicon.Database.Contexts;
public class AppDbContext : DbContext
{
    public DbSet<TestModel> TestModels => Set<TestModel>();
    public DbSet<App> Apps => Set<App>();
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

        SeedDatabase(mb);
    }

    private static void SeedDatabase(ModelBuilder mb)
    {

    }
}
