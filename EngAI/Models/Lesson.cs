namespace EngAI.Models;

public class Lesson
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public LessonType Type { get; set; }
    public LessonContent? Content { get; set; }

    public enum LessonType
    {
        Grammar, Vocabulary, Conversation, Pronunciation, Stress
    }
}
