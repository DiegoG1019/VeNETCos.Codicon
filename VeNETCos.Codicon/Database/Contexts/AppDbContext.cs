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
using VeNETCos.Codicon.UI.ViewModels;

namespace VeNETCos.Codicon.Database.Contexts;
public class AppDbContext : DbContext
{
    public static readonly Guid PrimaryBoxGuid = Guid.Parse("{11111111-1111-1111-1111-111111111111}");

    public DbSet<FileLink> FileLinks => Set<FileLink>();
    public DbSet<Box> Boxes => Set<Box>();

    public Box PrimaryBox => Boxes.First(x => x.Id == PrimaryBoxGuid);

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
#if DEBUG
        Seed(this);
#endif
    }

    public void LoadFileData(string folder, bool recursive = true)
    {
        Log.Information("Reading folder {folder} for file data to load into the database, interpreting folders as Boxes", folder);

        HashSet<string> readFolders = new();
        Dictionary<string, int> BoxNames = new();

        ReadFolder(folder, null);

        unsafe void ReadFolder(string f, ParentToChildrenRelationshipCollection<Box, Box>? parentChildrenCollection)
        {
            Queue<string> folders = new();

            var n = Path.GetFileName(f);
            n = BoxNames.TryGetValue(n, out int value) ? $"{n}-{value}" : n;
            BoxNames[n] = value + 1;

            int color = int.MaxValue;
            Span<byte> bytes = new Span<byte>(&color, sizeof(int))[1..];
            Random.Shared.NextBytes(bytes);

            Box box = new(Guid.NewGuid(), n, null, color);
            Boxes.Add(box);
            Log.Debug("Loading folder {f} under Box {guid}", f, box.Id);

            parentChildrenCollection?.Add(box);

            foreach (var dir in Directory.EnumerateDirectories(f)) 
                if (readFolders.Add(dir))
                    folders.Enqueue(dir);

            //Boxes.Add(box);
            SaveChanges();

            var pcc = BoxViewModel.CreateParentToChildrenRelationshipCollectionForBox(box.Id);
            while (folders.TryDequeue(out var fold))
                ReadFolder(fold, pcc);

            var c = BoxViewModel.CreateCrossRelationshipCollectionForBox(box.Id);
            foreach (var file in Directory.EnumerateFiles(f))
            {
                var fl = new FileLink(Guid.NewGuid(), file);
                Log.Verbose("Loading file {file} under FileLink {fguid} in Box {bguid}", file, fl.Id, box.Id);
                c.Add(fl);
            }
        }
    }

    public override int SaveChanges()
    {
        if (ChangeTracker.HasChanges())
        {
            var primaryBox = Boxes.FirstOrDefault(x => x.Id == PrimaryBoxGuid);
            if (primaryBox is null)
            {
                primaryBox = new Box(PrimaryBoxGuid, "Primary Box", "The Box where it All starts", 0);
                Boxes.Add(primaryBox);
            }

            foreach (var box in ChangeTracker.Entries<Box>().Where(x => x.Entity.Id != PrimaryBoxGuid && x.Entity.Parent is null).Select(x => x.Entity))
            {
                box.Parent = primaryBox;
                primaryBox.Children.Add(box);
            }
        }

        return base.SaveChanges();
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        base.OnModelCreating(mb);
        ConfigureAppModel(mb.Entity<FileLink>());
        ConfigureBoxModel(mb.Entity<Box>());
        mb.Entity<Box>().HasData(new Box(PrimaryBoxGuid, "Primary Box", "The Box where it All starts", 0));
    }

    private static void ConfigureBoxModel(EntityTypeBuilder<Box> mb)
    {
        mb.HasKey(x => x.Id);
        mb.HasMany(x => x.FileLinks).WithMany(x => x.Boxes);
        mb.HasOne(x => x.Parent).WithMany(x => x.Children);
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

            context.Database.EnsureCreated();

            if (context.Boxes.Any(x => x.Id != PrimaryBoxGuid) || context.FileLinks.Any())
            {
                Log.Information("Attempted to seed file data for databasedata; but the database already has data");
                return;
            }

            Log.Information("Seeding file data for database");

            Random rand = new();

            string dir = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "torako", AppConfiguration.UserProfile, "garbagedat");
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

            int maxd = rand.Next(2, 4);
            CreateFolders(dir, maxd);

            Log.Information("Finished seeding file data for database");

            context.LoadFileData(dir);

            void CreateFolders(string head, int depth)
            {
                int folders = rand.Next(1, 2);
                if (folders <= 0) return;

                Log.Debug("Creating {folders} folders at depth {depth} under {head}", folders, depth, head);
                for (int f = 0; f < folders; f++)
                {
                    string fname = NameGenerator.NextName;
                    int files = rand.Next(1, 10);
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
