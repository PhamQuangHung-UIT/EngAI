using Newtonsoft.Json.Serialization;

namespace EngAI.Utils;

public class CustomContractResolver : DefaultContractResolver
{
    public CustomContractResolver()
    {
        NamingStrategy = new SnakeCaseNamingStrategy
        {
            OverrideSpecifiedNames = true
        };
    }
}
