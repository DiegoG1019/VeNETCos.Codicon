namespace VeNETCos.Codicon.Database.Models;
public class AppBox :
    IToManyRelation<BoxedApp>
{
    public Guid Id { get; init; }
    public virtual ICollection<BoxedApp> Apps { get; init; } = new HashSet<BoxedApp>();
    public virtual ICollection<AppBox> Children { get; init; } = new HashSet<AppBox>();

    public AppBox? Parent { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int Color { get; set; }

    public AppBox(Guid id, string? title, string? description, int color)
    {
        Id = id;
        Title = title;
        Description = description;
        Color = color;
    }

    ICollection<BoxedApp> IToManyRelation<BoxedApp>.Relation => Apps;
}
