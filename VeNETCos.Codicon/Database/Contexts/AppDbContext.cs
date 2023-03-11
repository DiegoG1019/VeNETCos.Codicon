using System.Diagnostics;
using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Extensions.Logging;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Services.Static;
using VeNETCos.Codicon.Types;

namespace VeNETCos.Codicon.Database.Contexts;
public class AppDbContext : DbContext
{
    public DbSet<FileLink> FileLinks => Set<FileLink>();
    public DbSet<Box> Boxes => Set<Box>();

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
#if DEBUG
        Database.EnsureDeleted();
        Database.EnsureCreated();
        Seed(this);
#endif
    }

    public void LoadFileData(string folder, bool recursive = true)
    {
        Log.Information("Reading folder {folder} for file data to load into the database, interpreting folders as Boxes", folder);

        HashSet<string> readFolders = new();
        Dictionary<string, int> BoxNames = new();

        ReadFolder(folder, null);

        void ReadFolder(string f, ParentToChildrenRelationshipCollection<Box, Box>? parentChildrenCollection)
        {
            Queue<string> folders = new();

            var n = Path.GetFileName(f);
            n = BoxNames.TryGetValue(n, out int value) ? $"{n}-{value}" : n;
            BoxNames[n] = value + 1;

            Box box = new(Guid.NewGuid(), n, null, 0);
            Boxes.Add(box);
            Log.Debug("Loading folder {f} under Box {guid}", f, box.Id);

            if (parentChildrenCollection is not null)
                parentChildrenCollection.Add(box);

            foreach (var dir in Directory.EnumerateDirectories(f)) 
                if (readFolders.Add(dir))
                    folders.Enqueue(dir);

            var pcc = new ParentToChildrenRelationshipCollection<Box, Box>(this, box);
            while (folders.TryDequeue(out var fold))
                ReadFolder(fold, pcc);

                var c = new CrossRelationshipCollection<FileLink, Box>(this, box);
            foreach (var file in Directory.EnumerateFiles(f))
            {
                var fl = new FileLink(Guid.NewGuid(), file);
                Log.Verbose("Loading file {file} under FileLink {fguid} in Box {bguid}", file, fl.Id, box.Id);
                c.Add(fl);
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);
        ConfigureAppModel(mb.Entity<FileLink>());
        ConfigureBoxModel(mb.Entity<Box>());
    }

    private static void ConfigureBoxModel(EntityTypeBuilder<Box> mb)
    {
        mb.HasKey(x => x.Id);
        mb.HasMany(x => x.FileLinks).WithMany(x => x.Boxes);
        mb.HasOne(x => x.Parent).WithMany(x => x.Children);
        mb.Navigation(x => x.FileLinks).AutoInclude();
    }

    private static void ConfigureAppModel(EntityTypeBuilder<FileLink> mb)
    {
        mb.HasKey(x => x.Id);
    }

#if DEBUG

    private static bool isSeeded;
    private static readonly object sync = new();
    private static void Seed(AppDbContext context)
    {
        lock (sync)
        {
            if (isSeeded) return;
            isSeeded = true;

            if (context.Boxes.Any() || context.FileLinks.Any())
            {
                Log.Information("Attempted to seed file data for databasedata; but the database already has data");
                return;
            }

            Log.Information("Seeding file data for database");

            Random rand = new();

            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "torako", "debug", "garbagedat");
            if (Directory.Exists(dir)) 
                Directory.Delete(dir, true);
            Directory.CreateDirectory(dir);

            byte[] bytes = new byte[1000];
            rand.NextBytes(bytes);
            
            int fileCount = 0;

            WriteAndGetNewGarbage();
            WriteAndGetNewGarbage();
            WriteAndGetNewGarbage();

            WriteAndGetNewGarbage("First");
            WriteAndGetNewGarbage("First");
            WriteAndGetNewGarbage("First");

            int maxd = rand.Next(4, 7);
            CreateFolders(dir, maxd);

            Log.Information("Finished seeding file data for database");

            context.LoadFileData(dir);

            void CreateFolders(string head, int depth)
            {
                int folders = rand.Next(0, 5);
                if (folders <= 0) return;

                Log.Debug("Creating {folders} folders at depth {depth} under {head}", folders, depth, head);
                for (int f = 0; f < folders; f++)
                {
                    string fname = NameGenerator.NextName;
                    int files = rand.Next(0, 10);
                    string fd = Path.Combine(head, fname);

                    Directory.CreateDirectory(fd);
                    for (int d = 0; d < files; d++)
                        WriteAndGetNewGarbage(fd);

                    Log.Debug("Created {fd} with {files} garbage files for testing", fd, files);

                    if (depth > 0)
                        CreateFolders(fd, depth - 1);
                }
            }

            void WriteAndGetNewGarbage(string? folder = null)
            {
                var wd = Path.Combine(dir, folder ?? "");
                Directory.CreateDirectory(wd);

                var fn = Path.Combine(wd, $"file{fileCount++}.gbg");

                File.WriteAllBytes(fn, bytes);
                Log.Verbose("Written test file {fn} with garbage data", fn);
                rand.NextBytes(bytes);
            }
        }
    }

#endif
}
