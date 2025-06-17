namespace EngAI.Models;

public class GrammarLessonContent : LessonContent
{
    public required string GrammarTopic { get; set; }
    public string CreatePrompt(string promptTemplate) => string.Format(promptTemplate, Topic, GrammarTopic);
}
