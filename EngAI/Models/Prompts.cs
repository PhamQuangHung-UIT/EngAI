namespace EngAI.Models;

public class Prompts
{
    public required string GrammarPrompt { get; set; }
    public required string VocabularyPrompt { get; set; }
    public required string ConversationPrompt { get; set; }
    public required string PronunciationPrompt { get; set; }
    public required string WordStressPrompt { get; set; }
}
