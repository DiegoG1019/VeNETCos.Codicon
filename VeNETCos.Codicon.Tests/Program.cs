using Microsoft.EntityFrameworkCore;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;

namespace VeNETCos.Codicon.Tests;

internal class Program
{
    static void Main(string[] args)
    {
        using var services = AppServices.GetServices<AppDbContext>().Get(out var context);

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
