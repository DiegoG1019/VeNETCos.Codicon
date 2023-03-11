using System.Diagnostics.CodeAnalysis;

namespace VeNETCos.Codicon.Database.Models;
public class FileLink :
    IToManyRelation<Box>
{
    private string path;

    public Guid Id { get; init; }
    public virtual ICollection<Box> Boxes { get; init; } = new HashSet<Box>();

    [MemberNotNull(nameof(path))]
    public string Path 
    { 
        get => path ?? throw new InvalidOperationException("path has not been set"); 
        set => path = value ?? throw new ArgumentNullException(nameof(value)); 
    }

    public FileLink(Guid id, string path)
    {
        Id = id;
        Path = path ?? throw new ArgumentNullException(nameof(path));
    }

    ICollection<Box> IToManyRelation<Box>.Relation => Boxes;
}
