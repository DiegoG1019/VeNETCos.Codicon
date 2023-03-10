using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeNETCos.Codicon;
public static class Helper
{
}

public class StringInvariantIgnoreCaseComparison : IEqualityComparer<string>
{
    public bool Equals(string? x, string? y)
        => x.Equals(y, StringComparison.InvariantCultureIgnoreCase);

    public int GetHashCode([DisallowNull] string obj)
        => obj.ToLowerInvariant().GetHashCode();

    private StringInvariantIgnoreCaseComparison() { }

    public static StringInvariantIgnoreCaseComparison Instance { get; } = new();
}
