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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VeNETCos.Codicon.UI.ViewModels;

namespace VeNETCos.Codicon.UI.Controls;
/// <summary>
/// Interaction logic for FileLinkView.xaml
/// </summary>
public partial class FileLinkView : UserControl
{
    public FileLinkViewModel DataModel => (FileLinkViewModel)DataContext;

    public FileLinkView()
    {
        InitializeComponent();

        Drop += FileLinkView_Drop;
    }

    private void FileLinkView_Drop(object sender, DragEventArgs e)
    {
        
    }
}
