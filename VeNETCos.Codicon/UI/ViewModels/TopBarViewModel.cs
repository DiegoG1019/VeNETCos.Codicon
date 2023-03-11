using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;

namespace VeNETCos.Codicon.UI.ViewModels;

public abstract class TopBarViewModel : BaseViewModel, INotifyPropertyChanged
{
    private string? lastError;
    private readonly List<string> errors = new();

    private WindowState _windowState;
    public WindowState WindowState
    {
        get
        { return _windowState; }
        set
        {
            _windowState = value;
            NotifyPropertyChanged(nameof(WindowState));
        }
    }

    private ICommand _minimizeCommand;
    public ICommand MinimizeCommand
    {
        get
        {
            if (_minimizeCommand == null)
            {
                _minimizeCommand = new RelayCommand(param => this.Minimize(), param => true);
            }
            return _minimizeCommand;
        }
    }
    private void Minimize()
    {
        WindowState = WindowState.Minimized;
    }

}
