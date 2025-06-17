using System.ComponentModel.DataAnnotations.Schema;

namespace EngAI.Models;

public class UserDTO
{
    [Column("user_id")]
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    [Column("avatar_url")]
    public string? AvatarUrl { get; set; }

    public List<string>? Hobbies { get; set; }
}

public class User
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Email { get; set; }

    public string? AvatarUrl { get; set; }

    public List<string>? Hobbies { get; set; }

    public bool ChatSessionAvailable { get; set; } = false;
}
