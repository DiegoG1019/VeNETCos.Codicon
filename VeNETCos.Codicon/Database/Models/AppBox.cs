namespace VeNETCos.Codicon.Database.Models;
public class AppBox
{
    public Guid Id { get; set; }

    public AppBox? Parent { get; set; }
    public List<App> Apps { get; set; }
    string Tile { get; set; }
    string Description { get; set; }
    string Color { get; set; }
}
