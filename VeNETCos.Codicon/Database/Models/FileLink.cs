using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VeNETCos.Codicon.Database.Models;
public class FileLink :
    IID
{
    private string path;

    public Guid Id { get; init; }

    [MemberNotNull(nameof(path))]
    public string Path
    {
        get => path ?? throw new InvalidOperationException("path has not been set");
        set
        {
            path = value ?? throw new ArgumentNullException(nameof(value));
            name = null;
            str = null;
        }
    }

    private string? str;
    private string? name;

    public override string ToString() => str ??= $"{Name} ({Id})";

    [IgnoreDataMember]
    public string Name => name ??= System.IO.Path.GetFileName(Path);

    public FileLink(Guid id, string path)
    {
        Id = id;
        Path = path ?? throw new ArgumentNullException(nameof(path));
    }

    public void Open()
    {
        new Process { StartInfo = new ProcessStartInfo(Path) { UseShellExecute = true } }.Start();
    }
}
