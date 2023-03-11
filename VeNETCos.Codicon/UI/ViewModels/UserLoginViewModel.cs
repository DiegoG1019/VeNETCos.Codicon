using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeNETCos.Codicon.UI.ViewModels;
public class UserLoginViewModel : BaseViewModel
{
    private string? name;

    public string? Name
    {
        get => name;
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                ClearModelErrors();
                AddModelError(Language.Errors.UsernameNullError);
                return;
            }

            NotifyPropertyChanged(ref name, value);
        }
    }
}
