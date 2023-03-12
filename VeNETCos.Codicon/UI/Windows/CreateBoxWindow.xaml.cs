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
using VeNETCos.Codicon.UI.ViewModels;

namespace VeNETCos.Codicon.UI.Windows;
/// <summary>
/// Lógica de interacción para CreateBoxWindow.xaml
/// </summary>
public partial class CreateBoxWindow : Window
{
    public CreateBoxWindow(CreateBoxViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }

    private void CreateButton_Click(object sender, RoutedEventArgs e)
    {
      
        
       
       CreateBoxViewModel dataContext = (CreateBoxViewModel)DataContext;
       Box newBox = new Box(new Guid(), dataContext.Name, dataContext.Description,0);
       Box currentBox = BoxWindow.ActiveInstance.DataModel.CurrentBox.box;
       newBox.Parent = currentBox;

        using var services = AppServices.GetServices<AppDbContext>().Get(out var context);
        context.Boxes.Add(newBox);
       currentBox.Children.Add(newBox);
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
