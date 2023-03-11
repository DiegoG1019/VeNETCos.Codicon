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
        ModelCollectionsTest();   
    }

    static void ModelCollectionsTest()
    {
        using var services = AppServices.GetServices<AppDbContext>().Get(out var context);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.AppBoxes.Add(new AppBox(Guid.NewGuid(), "cajita dinda", "amo a mi cajita", 0));
        context.BoxedApps.Add(new BoxedApp(Guid.NewGuid(), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Archivo.txt")));
        context.SaveChanges();

        var box = context.AppBoxes.First();
        var app = context.BoxedApps.First();

        var appmodel = new BoxedAppViewModel(context, app);
        var boxmodel = new AppBoxViewModel(context, box);
        appmodel.Boxes.Add(boxmodel);

        foreach (var b in appmodel.Boxes)
            Console.WriteLine(b.Title ?? "No Title");
        foreach (var a in boxmodel.Apps)
            Console.WriteLine(a.Path);
    }

    static void DbCollectionsTest()
    {
        using var services = AppServices.GetServices<AppDbContext>().Get(out var context);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.AppBoxes.Add(new AppBox(Guid.NewGuid(), "cajita dinda", "amo a mi cajita", 0));
        context.BoxedApps.Add(new BoxedApp(Guid.NewGuid(), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Archivo.txt")));
        context.SaveChanges();

        var box = context.AppBoxes.First();
        var app = context.BoxedApps.First();

        var x = new CrossRelationshipCollection<BoxedApp, AppBox>(context, box);
        var newApp = new BoxedApp(Guid.NewGuid(), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Archivo.txt"));

        x.Add(app);
        x.Add(newApp);

        foreach (var i in x)
            Console.WriteLine(x.Count);
    }

    static void DbTest()
    {
        using var services = AppServices.GetServices<AppDbContext>().Get(out var context);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.AppBoxes.Add(new AppBox(Guid.NewGuid(), "Mi caja", "Mi cajita linda", 0));
        context.BoxedApps.Add(new BoxedApp(Guid.NewGuid(), Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Archivo.txt")));
        context.SaveChanges();

        Console.WriteLine(context.AppBoxes.Count());

        AppBox box = context.AppBoxes.First();
        BoxedApp app = context.BoxedApps.First();

        box.Apps.Add(app);
        app.Boxes.Add(box);

        context.SaveChanges();

        context.AppBoxes.ForEachAsync(box =>
        {
            Console.WriteLine(box.Id);
            Console.WriteLine(box.Apps.Count());
        });
    }
}
