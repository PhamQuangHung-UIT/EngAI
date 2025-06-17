using Newtonsoft.Json;

namespace EngAI.Models.Gemini;

public class GeminiRequest
{
    [JsonProperty("contents")]
    public required List<Content> Contents { get; set; }
}

public class Content
{
    public required string Role { get; set; }

    public List<Part> Parts { get; set; } = [];
}

public class Part
{
    public string Text { get; set; } = string.Empty;
}
