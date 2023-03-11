using Microsoft.EntityFrameworkCore;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;

namespace VeNETCos.Codicon.Tests;


internal class Program
{
    static void Main(string[] args)
    {
        using var services = AppServices.GetServices<AppDbContext>().Get(out var context);

        //context.AppBoxes.Add(new AppBox(Guid.NewGuid(), "Mi caja", "Mi cajita linda", 0));
        //context.BoxedApps.Add(new BoxedApp(Guid.NewGuid(), "\"C:\\Users\\Personal\\Desktop\\Archivo.txt\""));
        //context.SaveChanges();

        Console.WriteLine(context.AppBoxes.Count());

        //AppBox box = context.AppBoxes.Where(box => box.Id == Guid.Parse("39101818-9604-4d8f-8c42-7def77cdfd85")).First();
        //box.Apps.Add(context.BoxedApps.FirstOrDefault());

        BoxedApp app = context.BoxedApps.FirstOrDefault();
        app.Boxes.Add(context.AppBoxes.FirstOrDefault());

        context.SaveChanges();


        context.AppBoxes.ForEachAsync(box =>
        {
            Console.WriteLine(box.Id);
            Console.WriteLine(box.Apps.Count());
        });


    }
}
