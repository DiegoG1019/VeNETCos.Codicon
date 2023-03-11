using System.Windows;
using Serilog;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Services.Containers;
using VeNETCos.Codicon.UI.Pages;
using VeNETCos.Codicon.UI.ViewModels;

namespace VeNETCos.Codicon;
/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    readonly ILogger Log;

    public MainWindow()
    {
        InitializeComponent();
        Log = LoggerStore.GetLogger(this);
        Log.Information("Initialized MainWindow");
        DataContext = new MainModel(null);
    }
}
