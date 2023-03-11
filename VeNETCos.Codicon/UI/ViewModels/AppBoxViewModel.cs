using VeNETCos.Codicon.Database.Contexts;
using VeNETCos.Codicon.Database.Models;

namespace VeNETCos.Codicon.UI.ViewModels;

public class AppBoxViewModel : BaseViewModel
{
    private readonly AppDbContext context;
    private readonly AppBox box;
    private AppBoxViewModel? parent;

    public AppBoxViewModel(AppDbContext context, AppBox box)
    {
        this.context = context ?? throw new ArgumentNullException(nameof(context));
        this.box = box ?? throw new ArgumentNullException(nameof(box));
    }

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
}