using Microsoft.EntityFrameworkCore;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Types;
using VeNETCos.Codicon.UI.ViewModels;

namespace VeNETCos.Codicon.Tests;

internal class Program
{
    static void Main(string[] args)
    {
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
        foreach (var a in boxmodel.Apps)
            Console.WriteLine(a.Path);
    }

    static void DbCollectionsTest()
    {
        using var services = AppServices.GetServices<AppDbContext>().Get(out var context);

        //context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        //context.Boxes.Add(new Box(Guid.NewGuid(), "cajita dinda", "amo a mi cajita", 0));

        //var fl1 = new FileLink(Guid.NewGuid(), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Archivo1.txt"));
        //var fl2 = new FileLink(Guid.NewGuid(), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Archivo2.txt"));
        //var fl3 = new FileLink(Guid.NewGuid(), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Archivo3.txt"));

        //context.FileLinks.Add(fl1);
        //context.FileLinks.Add(fl2);

        //context.SaveChanges();

        var box = context.Boxes.First();
        var app = context.FileLinks.First();

        var x = new CrossRelationshipCollection<FileLink, Box>(context, box);

        foreach (var i in x)
        {
            Console.WriteLine("For file {0} ({1}):", i.Id, Path.GetFileName(i.Path));
            foreach (var b in i.Boxes)
                Console.WriteLine(b.Title);
        }
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

        box.Apps.Add(app);
        app.Boxes.Add(box);

        context.SaveChanges();

        context.Boxes.ForEachAsync(box =>
        {
            Console.WriteLine(box.Id);
            Console.WriteLine(box.Apps.Count);
        });
    }
}
