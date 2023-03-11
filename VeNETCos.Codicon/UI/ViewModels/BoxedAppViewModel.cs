using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;

namespace VeNETCos.Codicon.UI.ViewModels;

public class BoxedAppViewModel : BaseViewModel
{
    private readonly AppDbContext context;
    private readonly BoxedApp box;

    public BoxedAppViewModel(AppDbContext context, BoxedApp box)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.box = box ?? throw new ArgumentNullException(nameof(box));
    }

    public string Path
    {
        get => box.Path;
        set
        {
            if (box.Path == value) return;
            if (Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute) is false)
            {
                AddModelError(Language.Errors.InvalidPathError);
                return;
            }
            box.Path = NotifyPropertyChanged(box.Path, value);
        }
    }

    protected override bool PropertyHasChanged(string property)
    {
        context.SaveChanges();
        return true;
    }
}
