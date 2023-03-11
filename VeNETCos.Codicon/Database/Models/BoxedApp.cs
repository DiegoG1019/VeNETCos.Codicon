using System.Diagnostics.CodeAnalysis;

namespace VeNETCos.Codicon.Database.Models;
public class BoxedApp
{
    private string path;

    public Guid Id { get; init; }
    public virtual ICollection<AppBox> Boxes { get; init; } = new List<AppBox>();

    [MemberNotNull(nameof(path))]
    public string Path 
    { 
        get => path ?? throw new InvalidOperationException("path has not been set"); 
        set => path = value ?? throw new ArgumentNullException(nameof(value)); 
    }

    public BoxedApp(Guid id, string path)
    {
        Id = id;
        Path = path ?? throw new ArgumentNullException(nameof(path));
    }
}
