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

namespace VeNETCos.Codicon.UI.Pages
{
    /// <summary>
    /// Lógica de interacción para UserLoginView.xaml
    /// </summary>
    public partial class UserLoginView : Page
    {
        readonly ILogger Log;

        public MainModel DataModel { get; private set; }

        public UserLoginView()
        {
            InitializeComponent();
            Log = LoggerStore.GetLogger(this);
            Log.Information("Initialized Login Screen");

            DataContextChanged += UserLoginView_DataContextChanged;
        }

        private void UserLoginView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is MainModel mm)
            {
                DataModel = mm;
                return;
            }

            throw new InvalidOperationException("Cannot set the DataContext of a UserLoginView to anything other than a MainModel");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataModel.Validate() is false) return;

            ErrorLabel.Content = string.Join("\n*", DataModel.Errors);

            AppConfiguration.UserProfile = DataModel.UserLogin.Name!.Trim();
            Log.Information("Set user login information to {login}", AppConfiguration.UserProfile);

            DataModel.NavigateToMainScreen();
        }
    }
}
