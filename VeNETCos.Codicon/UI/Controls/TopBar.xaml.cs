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
using VeNETCos.Codicon.UI.Windows;

namespace VeNETCos.Codicon.UI.Controls
{
    /// <summary>
    /// Lógica de interacción para TopBar.xaml
    /// </summary>
    public partial class TopBar : UserControl
    {
        public TopBar()
        {
            InitializeComponent();
        }

        private void MinimizeBtn_Click(object sender, RoutedEventArgs e)
        {
            BoxWindow.ActiveInstance.WindowState = WindowState.Minimized;
        }
        private void MaximizeBtn_Click(object sender, RoutedEventArgs e) => BoxWindow.ActiveInstance.WindowState = BoxWindow.ActiveInstance.WindowState == WindowState.Maximized ? WindowState.Normal : WindowState.Maximized;

        private void CloseBtn_Click(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                BoxWindow.ActiveInstance.DragMove();
        }
    }
}
