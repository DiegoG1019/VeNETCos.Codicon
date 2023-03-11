using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VeNETCos.Codicon.Services.Static;
public static class NameGenerator
{
    private static readonly string[] Names = new string[]
    {
        "Manhattan",
        "Avocado",
        "Lettuce",
        "Chiller",
        "Important",
        "Alpha",
        "Omega",
        "Delta",
        "Val",
        "Super",
        "Frozen",
        "Celery",
        "Binary",
        "Beta",
        "Theta",
        "Xys",
        "Cecil",
        "Tetrad",
        "Tree",
        "Wing",
        "Left",
        "Noman"
    };

    public static Random Random { get; } = Random.Shared;

    public static string NextName => Names[Random.Next(0, Names.Length - 1)];
}
