namespace EngAI.Models;

public class Account
{
    public required string Id { get; set; }
    public required string Name { get; set; }
    public readonly List<string> Hobbies = [];
}
