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
using System.Windows.Threading;
using VeNETCos.Codicon.Database.Contexts;
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

        private DispatcherTimer Timer = new();

        public UserLoginView()
        {
            InitializeComponent();
            Log = LoggerStore.GetLogger(this);
            Log.Information("Initialized Login Screen");

            DataContextChanged += UserLoginView_DataContextChanged;
            LoginButton.KeyUp += LoginButton_KeyUp;

            Timer.Interval = TimeSpan.FromSeconds(1);
            Timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object? sender, EventArgs e)
        {
            if (AppStartingTask is not null && AppStartingTask.IsCompleted)
            {
                Timer.Stop();
                DataModel.NavigateToMainScreen();
            }
        }

        private void LoginButton_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.DeadCharProcessedKey is Key.Enter)
                ValidateForm();
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
            ValidateForm();
        }

        bool FormValidating = false;
        Task? AppStartingTask = null;
        private void ValidateForm()
        {
            if (DataModel.UserLogin.Validate() is false || FormValidating)
            {
                ErrorLabel.Content = string.Join("\n*", DataModel.UserLogin.Errors);
                return;
            }

            FormValidating = true;
            LoginButton.IsEnabled = false;
            var name = DataModel.UserLogin.Name!.Trim();

            Timer.Start();
            DataModel.UserLogin.IsLoading = FormValidating;
            AppStartingTask = Task.Run(() =>
            {
                AppConfiguration.UserProfile = name;
                Log.Information("Set user login information to {login}", AppConfiguration.UserProfile);
                using (AppServices.GetServices<AppDbContext>().Get(out var db))
                {
                    var c = db.PrimaryBox.Color;
                    db.PrimaryBox.Color = c;
                    db.SaveChanges();
                } // We force the app to use the DB, initializing it
            });
        }
    }
}
