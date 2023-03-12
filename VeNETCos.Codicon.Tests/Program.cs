using System.Text;
using Microsoft.EntityFrameworkCore;
using Serilog;
using VeNETCos.Codicon.Configuration;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Types;
using VeNETCos.Codicon.UI.ViewModels;

namespace VeNETCos.Codicon.Tests;

internal class Program
{
    static void Main(string[] args)
    {
        AppConfiguration.UserProfile = "debug";
        DbCollectionsTest();   
    }

    static void ModelCollectionsTest()
    {
        using var services = AppServices.GetServices<AppDbContext>().Get(out var context);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Boxes.Add(new Box(Guid.NewGuid(), "cajita dinda", "amo a mi cajita", 0));
        context.FileLinks.Add(new FileLink(Guid.NewGuid(), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Archivo.txt")));
        context.SaveChanges();

        var box = context.Boxes.First();
        var app = context.FileLinks.First();

        var appmodel = new FileLinkViewModel(context, app);
        var boxmodel = new BoxViewModel(context, box);
        appmodel.Boxes.Add(boxmodel);

        foreach (var b in appmodel.Boxes)
            Console.WriteLine(b.Title ?? "No Title");
        foreach (var a in boxmodel.LinkedFiles)
            Console.WriteLine(a.Path);
    }

    static void DbCollectionsTest()
    {
        using var services = AppServices.GetServices<AppDbContext>().Get(out var context);

        LoggerStore.GetLogger("");

        context.Database.EnsureCreated();

        StringBuilder sb = new();

        foreach (var b in context.Boxes)
        {
            sb.AppendLine($"Box > {b}");

            foreach (var fl in b.FileLinks)
                sb.AppendLine($" \tFileLink > {fl}");
        }

        Console.WriteLine(sb.ToString());
    }

    static void DbTest()
    {
        using var services = AppServices.GetServices<AppDbContext>().Get(out var context);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Boxes.Add(new Box(Guid.NewGuid(), "Mi caja", "Mi cajita linda", 0));
        context.FileLinks.Add(new FileLink(Guid.NewGuid(), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Archivo.txt")));
        context.SaveChanges();

        Console.WriteLine(context.Boxes.Count());

        Box box = context.Boxes.First();
        FileLink app = context.FileLinks.First();

        box.FileLinks.Add(app);
        app.Boxes.Add(box);

        context.SaveChanges();

        context.Boxes.ForEachAsync(box =>
        {
            Console.WriteLine(box.Id);
            Console.WriteLine(box.FileLinks.Count);
        });
    }
}
