namespace EngAI.Models;

public enum Role
{
    User,
    Model,
}

public abstract class ResponseBlock
{
    public Role Role { get; set; }

    public string ContentType { get; set; }
}
public class TextResponseBlock : ResponseBlock
{
    public string? Text { get; set; }

    public TextResponseBlock()
    {
        ContentType = "text";
    }
}

public class JsonResponseBlock : ResponseBlock
{
    public object? Json { get; set; }

    public JsonResponseBlock()
    {
        ContentType = "json";
    }
}