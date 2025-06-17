using Newtonsoft.Json;

namespace EngAI.Models;

public class ConversationObjectives
{
    public struct Task
    {
        [JsonProperty("task")]
        public string Name { get; set; }
        public bool IsComplete { get; set; }
    }
    public List<Task> Tasks { get; set; } = [];
}
