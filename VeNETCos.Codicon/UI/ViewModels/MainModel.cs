using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeNETCos.Codicon.UI.ViewModels;
public class MainModel : BaseViewModel
{
    private BoxViewModel? currentBox;
    private bool isInLoginScreen;

    public UserLoginViewModel UserLogin { get; } = new();

    public void NavigateToMainScreen() => NavigatingToMainScreen?.Invoke();

    public BoxViewModel? CurrentBox
    {
        get => currentBox;
        set => NotifyPropertyChanged(ref currentBox, value);
    }

    public MainModel(BoxViewModel? currentBox)
    {
        this.currentBox = currentBox;
    }

    public event Action? NavigatingToMainScreen;
}
