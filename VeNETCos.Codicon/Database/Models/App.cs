namespace VeNETCos.Codicon.Database.Models;
public class App
{
    public Guid Id { get; set; }
    public List<AppBox> Boxes { get; set; }
    string Path { get; set; }
}
