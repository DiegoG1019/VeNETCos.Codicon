using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VeNETCos.Codicon.UI.ViewModels;

namespace VeNETCos.Codicon.UI.Controls;
/// <summary>
/// Interaction logic for FileLinkItemView.xaml
/// </summary>
public partial class FileLinkItemView : UserControl
{
    public FileLinkViewModel DataModel => (FileLinkViewModel)DataContext;
    private readonly ILogger Log;

    public FileLinkItemView()
    {
        InitializeComponent();
        Log = LoggerStore.GetLogger(this);
    }

    private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
    {
        Log.Information("Trying to start a process for FileLink {file}", DataModel);
        try
        {
            Process.Start(DataModel.Path);
        }
        catch(Exception exc)
        {
            Log.Warning("Could not start a process for FileLink {file}", DataModel);
            return;
        }
        Log.Information("Succesfully started a process for FileLink {file}", DataModel);
    }
}
