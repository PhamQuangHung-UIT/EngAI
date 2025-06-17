using Newtonsoft.Json;

namespace EngAI.Models;

public class GeneratedModelThread
{
    [JsonProperty("blocks")]
    public List<ResponseBlock> ResponseBlocks { get; set; } = [];

    public static GeneratedModelThread FromText(string text)
    {
        var textBlocks = text.Split(["\r\n", "\n", "\r"], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        GeneratedModelThread response = new();

        foreach (var block in textBlocks)
        {
            if (block.StartsWith("```") && block.EndsWith("```"))
            {
                try
                {
                    block.Substring(3, block.Length - 6);
                    var json = JsonConvert.DeserializeObject(block);
                    var respondBlock = new JsonResponseBlock();

                    response.ResponseBlocks.Add(respondBlock);

                }
                catch (JsonException)
                {
                    var respondBlock = new TextResponseBlock() { Text = block };
                    response.ResponseBlocks.Add(respondBlock);
                }
            }
            else
            {
                var respondBlock = new TextResponseBlock() { Text = block };
                response.ResponseBlocks.Add(respondBlock);
            }
        }

        return response;
    }
}
