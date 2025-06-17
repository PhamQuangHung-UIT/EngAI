using System.ComponentModel;

namespace EngAI.Models;

[ImmutableObject(true)]
public class LessonDTO
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public Lesson.LessonType Type { get; set; }
    public string? Content { get; set; }
}
