using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Types;

namespace VeNETCos.Codicon.UI.ViewModels;

public class AppBoxViewModel : BaseViewModel//, IToManyRelationModelView<BoxedApp, AppBox>
{
    private readonly AppDbContext context;
    private readonly AppBox box;
    //private readonly CrossRelationshipCollection<BoxedApp, AppBox> relations;

    private AppBoxViewModel? parent;

    public AppBoxViewModel(AppDbContext context, AppBox box)
    {
        if (context.AppBoxes.Find(box.Id) is null)
        {
            context.AppBoxes.Add(box);
            context.SaveChanges();
        }

        //relations = new(context, box);
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.box = box ?? throw new ArgumentNullException(nameof(box));
        //Apps = new ModelCrossRelationCollection<BoxedAppViewModel, BoxedApp, AppBoxViewModel, AppBox>(relations, m => new BoxedAppViewModel(context, m));
    }

    public ICollection<BoxedAppViewModel> Apps { get; }

    public AppBoxViewModel? Parent
    {
        get => parent;
        set
        {
            if(parent == value) return;
            NotifyPropertyChanged(ref parent, value);
            box.Parent = parent?.box;
        }
    }

    public string? Title
    {
        get => box.Title;
        set => box.Title = NotifyPropertyChanged(box.Title, value);
    }

    public string? Description
    {
        get => box.Description;
        set => box.Description = NotifyPropertyChanged(box.Description, value);
    }

    public int Color
    {
        get => box.Color;
        set => box.Color = NotifyPropertyChanged(box.Color, value);
    }

    protected override bool PropertyHasChanged(string property)
    {
        context.SaveChanges();
        return true;
    }

    //IToManyRelation<BoxedApp> IToManyRelationModelView<BoxedApp, AppBox>.RelationModel => box;
    //AppBox IToManyRelationModelView<BoxedApp, AppBox>.Model => box;
}