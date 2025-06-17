namespace EngAI.Models.Request;

public class ThirdPartyLoginRequest
{
    public required string Provider { get; set; }
    public required string AccessToken { get; set; }
}