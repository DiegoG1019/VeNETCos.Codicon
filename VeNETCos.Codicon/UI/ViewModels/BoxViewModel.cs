using System.Windows.Media.Imaging;
using Microsoft.EntityFrameworkCore;
using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;
using VeNETCos.Codicon.Types;

namespace VeNETCos.Codicon.UI.ViewModels;

public class BoxViewModel : BaseViewModel, IToManyRelationModelView<FileLink, Box>, IModelView<Box>
{
    private readonly Guid boxId;
    private readonly CrossRelationshipCollection<FileLink, Box> relations;
    private readonly ParentToChildrenRelationshipCollection<Box, Box> children;

    private BoxViewModel? parent;

    public BoxViewModel(Guid box)
    {
        using var services = AppServices.GetServices<AppDbContext>().Get(out var context);
        var bbox = context.Boxes.Include(x => x.Children).Include(x => x.FileLinks).FirstOrDefault(x => x.Id == box) ?? throw new ArgumentException("Could not find a box by the given Id", nameof(box));

        Image = new BitmapImage(new("/UI/Resources/TORAKO-TOPOPEN.png", UriKind.Relative));

        boxId = box;

        relations = CreateCrossRelationshipCollectionForBox(box);
        children = CreateParentToChildrenRelationshipCollectionForBox(box);

        LinkedFiles = new ModelCrossRelationCollection<FileLinkViewModel, FileLink, BoxViewModel, Box>(relations, m => new FileLinkViewModel(m.Id));
        Children = new ModelParentToChildrenRelationCollection<BoxViewModel, Box, BoxViewModel, Box>(children, m => new BoxViewModel(m.Id));

        var c = $"#{bbox.Color.ToString("X").PadRight(8, '0')}";
        FillColor = (SolidColorBrush)new BrushConverter().ConvertFrom(c)!;

        titleCache = bbox.Title;
        descrCache = bbox.Description;
        colorCache = bbox.Color;
    }

    public static CrossRelationshipCollection<FileLink, Box> CreateCrossRelationshipCollectionForBox(Guid boxId)
        => new(
            boxId, 
            (c, i) => c.Boxes.Include(x => x.FileLinks).First(x => x.Id == i),
            (c, i) => c.FileLinks.Include(x => x.Boxes).First(x => x.Id == i)
        );

    public static ParentToChildrenRelationshipCollection<Box, Box> CreateParentToChildrenRelationshipCollectionForBox(Guid boxId)
        => new(
            boxId,
            (c, i) => c.Boxes.Include(x => x.Children).First(x => x.Id == i),
            (c, i) => c.Boxes.First(x => x.Id == i),
            (c, i) => c.Boxes.Include(x => x.Parent).First(x => x.Id == i),
            (c, b) => c.Boxes.Contains(b)
        );

    public ModelCrossRelationCollection<FileLinkViewModel, FileLink, BoxViewModel, Box> LinkedFiles { get; }

    public ImageSource Image { get; }

    public BoxViewModel? Parent
    {
        get => parent;
        set
        {
            using(AppServices.GetDbContext(out var context))
            {
                var box = context.Boxes.Include(x => x.Parent).First(x => x.Id == boxId);

                if (value is not null)
                {
                    var newb = context.Boxes.First(x => x.Id == value.boxId);

                    NotifyPropertyChanged(ref parent, value);
                    box.Parent = newb;
                }
                else
                {
                    NotifyPropertyChanged(ref parent, null);
                    box.Parent = null;
                }
            }
        }
    }

    public ModelParentToChildrenRelationCollection<BoxViewModel, Box, BoxViewModel, Box> Children { get; }

    protected override void OnInit()
    {
        Log.Information("Initialized new model for Box {box}", boxId);
    }

    public Brush FillColor { get; private set; }

    private string? titleCache;
    public string? Title
    {
        get => titleCache;
        set
        {
            if (value == titleCache) return;

            using (AppServices.GetDbContext(out var context))
            {
                var box = context.Boxes.First(x => x.Id == boxId);
                box.Title = value;
                titleCache = value;
                NotifyPropertyChanged();
                context.SaveChanges();
            }
        }
    }

    private string? descrCache;
    public string? Description
    {
        get => descrCache;
        set
        {
            if (value == descrCache) return;

            using (AppServices.GetDbContext(out var context))
            {
                var box = context.Boxes.First(x => x.Id == boxId);
                box.Description = value;
                descrCache = value;
                NotifyPropertyChanged();
                context.SaveChanges();
            }
        }
    }

    private int colorCache;
    public int Color
    {
        get => colorCache;
        set
        {
            if(colorCache == value) return;

            using (AppServices.GetDbContext(out var context))
            {
                var box = context.Boxes.First(x => x.Id == boxId);
                box.Color = value;
                colorCache = value;
                NotifyPropertyChanged();
                context.SaveChanges();
            }

            FillColor = (SolidColorBrush)new BrushConverter().ConvertFrom($"#{colorCache.ToString("X").PadRight(8, '0')}")!;
            NotifyPropertyChanged(nameof(FillColor));
        }
    }

    public void Update()
    {
        Children.Update();
        LinkedFiles.Update();
    }

    Guid IModelView<Box>.ModelId => boxId;
}