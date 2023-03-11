using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Types;
using VeNETCos.Codicon.UI.Pages;

namespace VeNETCos.Codicon.UI.Windows;
/// <summary>
/// Lógica de interacción para BoxWindow.xaml
/// </summary>
public partial class BoxWindow : Window
{
    public BoxWindow()
    {
        InitializeComponent();
        UserLoginView view = new UserLoginView();
        Content = view;
        //DbCollectionsTest();
    }

    static void DbCollectionsTest()
    {
        using var services = AppServices.GetServices<AppDbContext>().Get(out var context);

        context.Database.EnsureDeleted();
        context.Database.EnsureCreated();

        context.Boxes.Add(new Box(Guid.NewGuid(), "cajita dinda", "amo a mi cajita", 0));

        var fl1 = new FileLink(Guid.NewGuid(), System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Archivo1.txt"));
        var fl2 = new FileLink(Guid.NewGuid(), System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Archivo2.txt"));
        var fl3 = new FileLink(Guid.NewGuid(), System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "Archivo3.txt"));

        context.FileLinks.Add(fl1);
        context.FileLinks.Add(fl2);

        context.SaveChanges();

        var box = context.Boxes.First();
        var app = context.FileLinks.First();

        var x = new CrossRelationshipCollection<FileLink, Box>(context, box)
        {
            fl1,
            fl2,
            fl3
        };

        foreach (var i in x)
        {
            foreach (var b in i.Boxes)
                Console.WriteLine(b.Title);
        }
    }

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }

    private void MinimizeBtn_Click(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;
    private void MaximizeBtn_Click(object sender, RoutedEventArgs e) => WindowState = WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;
    

    private void CloseBtn_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();
    
}
