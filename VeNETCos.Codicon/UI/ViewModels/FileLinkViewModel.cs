using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Types;

namespace VeNETCos.Codicon.UI.ViewModels;

public class FileLinkViewModel : BaseViewModel, IModelView<FileLink>
{
    private readonly AppDbContext context;
    private readonly FileLink fileLink;

    private static ImageSource DefaultIcon;

    public FileLinkViewModel(AppDbContext context, FileLink fileLink)
    {
        //DefaultIcon ??= new BitmapImage(new Uri("/UI/Resources/TORAKO-PAPER.png"));

        if (context.FileLinks.Find(fileLink.Id) is null)
        {
            context.FileLinks.Add(fileLink);
            context.SaveChanges();
        }

        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.fileLink = fileLink ?? throw new ArgumentNullException(nameof(fileLink));
    }

    public string Path
    {
        get => fileLink.Path;
        set
        {
            if (fileLink.Path == value) return;
            if (Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute) is false)
            {
                AddModelError(Language.Errors.InvalidPathError);
                return;
            }
            fileLink.Path = value;
            Icon = IconStore.GetIcon(Path) ?? DefaultIcon;

            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(Name));
            NotifyPropertyChanged(nameof(Icon));
        }
    }

    public ImageSource Icon { get; private set; }

    public string Name => fileLink.Name; 
    
    protected override void OnInit()
    {
        Log.Information("Initialized new model for FileLink {fileLink}", fileLink);
    }

    protected override bool PropertyHasChanged(string property)
    {
        context.SaveChanges();
        return true;
    }

    FileLink IModelView<FileLink>.Model => fileLink;
}
