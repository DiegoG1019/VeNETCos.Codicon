using System.Diagnostics.CodeAnalysis;

namespace VeNETCos.Codicon.Database.Models;
public class BoxedApp
{
    private string path;

    public Guid Id { get; init; }
    public virtual ICollection<AppBox> Boxes { get; init; }

    [MemberNotNull(nameof(path))]
    public string Path 
    { 
        get => path ?? throw new InvalidOperationException("path has not been set"); 
        set => path = value ?? throw new ArgumentNullException(nameof(value)); 
    }

    public BoxedApp(Guid id, ICollection<AppBox> boxes, string path)
    {
        Id = id;
        Boxes = boxes ?? throw new ArgumentNullException(nameof(boxes));
        Path = path ?? throw new ArgumentNullException(nameof(path));
    }
}
