using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Types;

namespace VeNETCos.Codicon.UI.ViewModels;

public class BoxViewModel : BaseViewModel, IToManyRelationModelView<FileLink, Box>, IModelView<Box>
{
    private readonly AppDbContext context;
    public readonly Box box;
    private readonly ParentToChildrenRelationshipCollection<Box, Box> children;

    private BoxViewModel? parent;

    public BoxViewModel(AppDbContext context, Box box)
    {
        if (context.Boxes.Find(box.Id) is null)
        {
            context.Boxes.Add(box);
            context.SaveChanges();
        }

        Image = new BitmapImage(new("/UI/Resources/TORAKO-TOPOPEN.png", UriKind.Relative));
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.box = box ?? throw new ArgumentNullException(nameof(box));
        children = new(context, box);
        
        LinkedFiles = new ModelSingleRelationCollection<FileLinkViewModel, FileLink, BoxViewModel, Box>(box, m => new FileLinkViewModel(context, m));

        Children = new ModelParentToChildrenRelationCollection<BoxViewModel, Box, BoxViewModel, Box>(
            children,
            m => new BoxViewModel(context, context.Boxes.Include(x => x.Children).Include(x => x.FileLinks).First(x => x.Id == m.Id))
        );

        var c = $"#{box.Color.ToString("X").PadRight(8, '0')}";
        FillColor = (SolidColorBrush)new BrushConverter().ConvertFrom(c)!;
    }

    public ICollection<FileLinkViewModel> LinkedFiles { get; }

    public ImageSource Image { get; }

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

    protected override void OnInit()
    {
        Log.Information("Initialized new model for Box {box}", box);
    }

    public Brush FillColor { get; private set; }

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
        set
        {
            if(box.Color == value) return;
            box.Color = value;
            NotifyPropertyChanged();

            FillColor = (SolidColorBrush)new BrushConverter().ConvertFrom($"#{box.Color.ToString("X").PadRight(8, '0')}")!;
            NotifyPropertyChanged(nameof(FillColor));
        }
    }

    protected override bool PropertyHasChanged(string property)
    {
        context.SaveChanges();
        return true;
    }

    IToManyRelation<FileLink> IToManyRelationModelView<FileLink, Box>.RelationModel => box;
    Box IToManyRelationModelView<FileLink, Box>.Model => box;

    Box IModelView<Box>.Model => box;
}