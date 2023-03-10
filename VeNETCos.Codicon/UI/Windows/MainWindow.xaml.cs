using System.Windows;
using Serilog;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Services.Containers;

namespace VeNETCos.Codicon;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        AyoINeedTheDatabase_EXAMPLE();
    }

    public void AyoINeedTheDatabase_EXAMPLE()
    {
        using var s = AppServices.GetServices<AppDbContext>().Get(out var context);

        context.TestModels.Add(new Database.Models.TestModel()); // yay

        context.SaveChanges(); // nice
    }
}
