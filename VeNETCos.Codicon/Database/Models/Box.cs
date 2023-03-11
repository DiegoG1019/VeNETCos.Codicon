namespace VeNETCos.Codicon.Database.Models;
public class Box :
    IToManyRelation<FileLink>,
    IOneToManyRelation<Box, Box>,
    IToOneRelation<Box>,
    IID
{
    public Guid Id { get; init; }
    public virtual ICollection<FileLink> FileLinks { get; init; } = new HashSet<FileLink>();
    public virtual ICollection<Box> Children { get; init; } = new HashSet<Box>();

    public Box? Parent { get; set; }
    public string? Title
    {
        get => title;
        set
        {
            if (title == value) return;
            title = value;
            str = null;
        }
    }
    public string? Description { get; set; }
    public int Color { get; set; }

    public Box(Guid id, string? title, string? description, int color)
    {
        Id = id;
        Title = title;
        Description = description;
        Color = color;
    }

    private string? str;
    private string? title;

    public override string ToString() => str ??= (Parent is Box p ? $"{Title} ({Id}); {p}" : $"{Title} ({Id})");

    ICollection<FileLink> IToManyRelation<FileLink>.Relation => FileLinks;

    ICollection<Box> IOneToManyRelation<Box, Box>.Many => Children;
    Box IOneToManyRelation<Box, Box>.One => this;

    Box? IToOneRelation<Box>.Related
    {
        get => Parent;
        set => Parent = value;
    }
}
