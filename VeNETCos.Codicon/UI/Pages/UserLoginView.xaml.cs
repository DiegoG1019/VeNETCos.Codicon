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

        public UserLoginViewModel DataModel { get; private set; }

        public UserLoginView(UserLoginViewModel model)
        {
            InitializeComponent();
            Log = LoggerStore.GetLogger(this);
            DataContext = new UserLoginViewModel();
            Log.Information("Initialized Login Screen");

            DataContext = DataModel = model;

            DataContextChanged += UserLoginView_DataContextChanged;
        }

        private void UserLoginView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is not UserLoginViewModel dm)
                throw new InvalidOperationException("Cannot set the DataContext of a UserLoginView to anything other than a UserLoginViewModel");
            DataModel = dm;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (DataModel.Validate() is false) return;

            AppConfiguration.UserProfile = DataModel.Name!.Trim();
            Log.Information("Set user login information to {login}", AppConfiguration.UserProfile);
        }
    }
}
