using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
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
using Microsoft.EntityFrameworkCore;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Services.Containers;
using VeNETCos.Codicon.Types;
using VeNETCos.Codicon.UI.Pages;
using VeNETCos.Codicon.UI.ViewModels;

namespace VeNETCos.Codicon.UI.Windows;
/// <summary>
/// Lógica de interacción para BoxWindow.xaml
/// </summary>
public partial class BoxWindow : Window
{
    [MemberNotNull(nameof(activeInstance))]
    public static BoxWindow ActiveInstance
        => activeInstance ?? throw new InvalidOperationException("The BoxWindow has not been initialized");
    
    readonly ILogger Log;
    private static BoxWindow? activeInstance;

    public MainModel DataModel => (MainModel)DataContext;

    readonly object MainContent;

    public BoxWindow()
    {
        InitializeComponent();
        activeInstance = this;

        Log = LoggerStore.GetLogger(this);
        Log.Information("Initialized BoxWindow");
        DataContext = new MainModel(null);
        MainContent = Content;

        Content = new UserLoginView() { DataContext = DataModel };

        DataModel.NavigatingToMainScreen += DataModel_NavigatingToMainScreen;
    }

    private void DataModel_NavigatingToMainScreen()
    {
        Content = MainContent;
        using (AppServices.GetServices<AppDbContext>().Get(out var context))
            DataModel.CurrentBox = new BoxViewModel(AppDbContext.PrimaryBoxGuid);
    }
}
