namespace VeNETCos.Codicon.Database.Models;
public class AppBox
{
    public Guid Id { get; init; }
    public virtual ICollection<BoxedApp> Apps { get; init; } = new List<BoxedApp>();

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
}
