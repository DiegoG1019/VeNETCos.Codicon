using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Types;

namespace VeNETCos.Codicon.UI.ViewModels;

public class FileLinkViewModel : BaseViewModel, IToManyRelationModelView<Box, FileLink>, IModelView<FileLink>
{
    public Guid FileLinkId { get; private set; }
    private readonly CrossRelationshipCollection<Box, FileLink> relations;

    private static ImageSource? DefaultIcon;

    public FileLinkViewModel(Guid fileLink)
    {
        //DefaultIcon ??= new BitmapImage(new Uri("/UI/Resources/TORAKO-PAPER.png"));
        using var services = AppServices.GetDbContext(out var context);
        var fl = context.FileLinks.Include(x => x.Boxes).FirstOrDefault(x => x.Id == fileLink) ?? throw new ArgumentException("Could not find a FileLink by the given ID", nameof(fileLink));

        this.FileLinkId = fileLink;

        relations = new(fileLink, (c, i) => c.FileLinks.First(x => x.Id == i), (c, i) => c.Boxes.Include(x => x.FileLinks).First(x => x.Id == i));

        Boxes = new ModelCrossRelationCollection<BoxViewModel, Box, FileLinkViewModel, FileLink>(relations, m => new BoxViewModel(m.Id));
        Path = fl.Path;
    }

    public ModelCrossRelationCollection<BoxViewModel, Box, FileLinkViewModel, FileLink> Boxes { get; }

    private string pathC;
    public string Path
    {
        get => pathC;
        set
        {
            if (pathC == value) return;
            pathC = value;
            Icon = IconStore.GetIcon(Path) ?? DefaultIcon;

            using (AppServices.GetDbContext(out var context))
            {
                var fl = context.FileLinks.First(x => x.Id == FileLinkId);
                fl.Path = value;
                Name = fl.Name;
                context.SaveChanges();
            }

            NotifyPropertyChanged();
            NotifyPropertyChanged(nameof(Name));
            NotifyPropertyChanged(nameof(Icon));
        }
    }

    public ImageSource? Icon { get; private set; }

    public string Name { get; private set; }
    
    protected override void OnInit()
    {
        Log.Information("Initialized new model for FileLink {fileLink}", FileLinkId);
    }

    protected override bool PropertyHasChanged(string property)
    {
        return true;
    }

    public void Update()
    {
        Boxes.Update();
    }

    Guid IModelView<FileLink>.ModelId => FileLinkId;
}
