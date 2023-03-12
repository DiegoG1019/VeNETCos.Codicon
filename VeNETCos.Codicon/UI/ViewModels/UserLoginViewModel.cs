using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeNETCos.Codicon.UI.ViewModels;
public class UserLoginViewModel : BaseViewModel
{
    private static readonly char[] InvalidChars = Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars()).Concat(new char[] { '\\', '/', '.' }).Distinct().ToArray();
    private string? name;

    private bool isLoading;
    public bool IsLoading
    {
        get => isLoading;
        set => NotifyPropertyChanged(ref isLoading, value);
    }


    public string? Name
    {
        get => name;
        set => NotifyPropertyChanged(ref name, value);
    }

    protected override bool Validating()
    {
        var name = Name?.Trim();
        if (string.IsNullOrWhiteSpace(name))
        {
            AddModelError(Language.Errors.UsernameNullError);
            return false;
        }

        foreach (char c in InvalidChars)
            if (name.Contains(c))
            {
                AddModelError(Language.Errors.InvalidUsernameError);
                return false;
            }

        return true;
    }
}
