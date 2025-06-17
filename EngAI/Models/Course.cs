namespace EngAI.Models;

public class Course
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public required string Description { get; set; }
    public string? ImageUrl { get; set; }
    public List<Unit> Units { get; } = [];
}
