using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Types;

namespace VeNETCos.Codicon.UI.ViewModels;

public class BoxedAppViewModel : BaseViewModel, IToManyRelationModelView<AppBox, BoxedApp>
{
    private readonly AppDbContext context;
    private readonly BoxedApp app;
    private readonly CrossRelationshipCollection<AppBox, BoxedApp> relations;

    public BoxedAppViewModel(AppDbContext context, BoxedApp app)
    {
        if (context.BoxedApps.Find(app.Id) is null)
        {
            context.BoxedApps.Add(app);
            context.SaveChanges();
        }

        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.app = app ?? throw new ArgumentNullException(nameof(app));
        relations = new(context, app);
        Boxes = new ModelCrossRelationCollection<AppBoxViewModel, AppBox, BoxedAppViewModel, BoxedApp>(relations, m => new AppBoxViewModel(context, m));
    }

    public ICollection<AppBoxViewModel> Boxes { get; }

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

    IToManyRelation<AppBox> IToManyRelationModelView<AppBox, BoxedApp>.RelationModel => app;
    BoxedApp IToManyRelationModelView<AppBox, BoxedApp>.Model => app;

    protected override bool PropertyHasChanged(string property)
    {
        context.SaveChanges();
        return true;
    }
}
