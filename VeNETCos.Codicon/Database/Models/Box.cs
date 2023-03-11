namespace VeNETCos.Codicon.Database.Models;
public class Box :
    IToManyRelation<FileLink>
{
    public Guid Id { get; init; }
    public virtual ICollection<FileLink> Apps { get; init; } = new HashSet<FileLink>();
    public virtual ICollection<Box> Children { get; init; } = new HashSet<Box>();

    public Box? Parent { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int Color { get; set; }

    public Box(Guid id, string? title, string? description, int color)
    {
        Id = id;
        Title = title;
        Description = description;
        Color = color;
    }

    ICollection<FileLink> IToManyRelation<FileLink>.Relation => Apps;
}
