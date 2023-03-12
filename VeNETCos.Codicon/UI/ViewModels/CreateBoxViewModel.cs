using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeNETCos.Codicon.UI.ViewModels;
public class CreateBoxViewModel : BaseViewModel
{
    private static readonly char[] InvalidChars = Path.GetInvalidPathChars().Concat(Path.GetInvalidFileNameChars()).Concat(new char[] { '\\', '/', '.' }).Distinct().ToArray();
    private string? name;
    public string? Name
    {
        get => name;
        set => NotifyPropertyChanged(ref name, value);
    }

    private string? description;

    public string? Description
    {
        get => description;
        set => NotifyPropertyChanged(ref description, value);
    }

    public string? color;
    public string? Color
    {
        get => color;
        set => NotifyPropertyChanged(ref color, value);
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
