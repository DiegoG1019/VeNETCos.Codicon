namespace VeNETCos.Codicon.Database.Models;
public class AppBox
{
    public Guid Id { get; init; }
    public virtual ICollection<BoxedApp> Apps { get; init; }

    public AppBox? Parent { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public int Color { get; set; }

    public AppBox(Guid id, ICollection<BoxedApp> apps, AppBox? parent, string? title, string? description, int color)
    {
        Id = id;
        Apps = apps ?? throw new ArgumentNullException(nameof(apps));
        Parent = parent;
        Title = title;
        Description = description;
        Color = color;
    }
}
