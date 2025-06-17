namespace EngAI.Models;

public class Unit
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public List<Lesson> Lessons { get; } = [];
}
