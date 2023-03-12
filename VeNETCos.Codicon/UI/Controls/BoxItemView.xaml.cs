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
using VeNETCos.Codicon.UI.Windows;

namespace VeNETCos.Codicon.UI.Controls;
/// <summary>
/// Interaction logic for BoxItemView.xaml
/// </summary>
public partial class BoxItemView : UserControl
{
    public BoxItemView()
    {
        InitializeComponent();
    }

    private void BgRect_MouseDown(object sender, MouseButtonEventArgs e)
    {
        BoxWindow.ActiveInstance.DataModel.CurrentBox = (BoxViewModel)DataContext;
    }
}
