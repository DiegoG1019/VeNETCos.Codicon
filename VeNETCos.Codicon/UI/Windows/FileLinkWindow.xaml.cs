using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
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
public partial class FileLinkWindow : Window
{
    readonly ILogger Log;

    public FileLinkViewModel DataModel => (FileLinkViewModel)DataContext;

    public FileLinkWindow(FileLinkViewModel model)
    {
        InitializeComponent();
        DataContext = model;
        Log = LoggerStore.GetLogger(this);
        Log.Information("Initialized new FileLinkWindow");
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
        if (e.ChangedButton == MouseButton.Left)
            DragMove();
    }

    private void TextBlock_MouseDown(object sender, MouseButtonEventArgs e)
    {
        Close();
    }
}
