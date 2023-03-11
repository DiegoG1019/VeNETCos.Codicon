using System.Diagnostics.CodeAnalysis;

namespace VeNETCos.Codicon;

public class StringInvariantIgnoreCaseComparison : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
        => x.Equals(y, StringComparison.InvariantCultureIgnoreCase);

    public int GetHashCode([DisallowNull] string obj)
        => obj.ToLowerInvariant().GetHashCode();

    private StringInvariantIgnoreCaseComparison() { }

    public static StringInvariantIgnoreCaseComparison Instance { get; } = new();
}
