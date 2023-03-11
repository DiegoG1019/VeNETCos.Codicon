using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Types;

namespace VeNETCos.Codicon.UI.ViewModels;

public class FileLinkViewModel : BaseViewModel, IToManyRelationModelView<Box, FileLink>
{
    private readonly AppDbContext context;
    private readonly FileLink app;
    private readonly CrossRelationshipCollection<Box, FileLink> relations;

    public FileLinkViewModel(AppDbContext context, FileLink app)
    {
        if (context.FileLinks.Find(app.Id) is null)
        {
            context.FileLinks.Add(app);
            context.SaveChanges();
        }

        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.app = app ?? throw new ArgumentNullException(nameof(app));
        relations = new(context, app);
        Boxes = new ModelCrossRelationCollection<BoxViewModel, Box, FileLinkViewModel, FileLink>(relations, m => new BoxViewModel(context, m));
    }

    public ICollection<BoxViewModel> Boxes { get; }

    public string Path
    {
        get => app.Path;
        set
        {
            if (app.Path == value) return;
            if (Uri.IsWellFormedUriString(value, UriKind.RelativeOrAbsolute) is false)
            {
                AddModelError(Language.Errors.InvalidPathError);
                return;
            }
            app.Path = NotifyPropertyChanged(app.Path, value);
        }
    }

    IToManyRelation<Box> IToManyRelationModelView<Box, FileLink>.RelationModel => app;
    FileLink IToManyRelationModelView<Box, FileLink>.Model => app;

    protected override bool PropertyHasChanged(string property)
    {
        context.SaveChanges();
        return true;
    }
}
