using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Types;

namespace VeNETCos.Codicon.UI.ViewModels;

public class BoxViewModel : BaseViewModel, IToManyRelationModelView<FileLink, Box>
{
    private readonly AppDbContext context;
    private readonly Box box;
    private readonly CrossRelationshipCollection<FileLink, Box> relations;
    private readonly ParentToChildrenRelationshipCollection<Box, Box> children;

    private BoxViewModel? parent;

    public BoxViewModel(AppDbContext context, Box box)
    {
        if (context.Boxes.Find(box.Id) is null)
        {
            context.Boxes.Add(box);
            context.SaveChanges();
        }

        relations = new(context, box);
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.box = box ?? throw new ArgumentNullException(nameof(box));
        children = new(context, box);
        Apps = new ModelCrossRelationCollection<FileLinkViewModel, FileLink, BoxViewModel, Box>(relations, m => new FileLinkViewModel(context, m));
        Children = new ModelParentToChildrenRelationCollection<BoxViewModel, Box, BoxViewModel, Box>(children, m => new BoxViewModel(context, m));
    }

    public ICollection<FileLinkViewModel> Apps { get; }

    public BoxViewModel? Parent
    {
        get => parent;
        set
        {
            if(parent == value) return;
            NotifyPropertyChanged(ref parent, value);
            box.Parent = parent?.box;
        }
    }

    public ModelParentToChildrenRelationCollection<BoxViewModel, Box, BoxViewModel, Box> Children { get; }

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

    IToManyRelation<FileLink> IToManyRelationModelView<FileLink, Box>.RelationModel => box;
    Box IToManyRelationModelView<FileLink, Box>.Model => box;
}