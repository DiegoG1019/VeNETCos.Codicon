using System.Diagnostics.CodeAnalysis;
using System.Security.RightsManagement;
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
    [MemberNotNull(nameof(activeInstance))]
    public static MainWindow ActiveInstance 
        => activeInstance ?? throw new InvalidOperationException("The MainWindow has not been initialized");

    readonly ILogger Log;
    private static MainWindow? activeInstance;

    public MainWindow()
    {
        InitializeComponent();
        activeInstance = this;

        Log = LoggerStore.GetLogger(this);
        Log.Information("Initialized MainWindow");
        DataContext = new MainModel(null);
    }
}
