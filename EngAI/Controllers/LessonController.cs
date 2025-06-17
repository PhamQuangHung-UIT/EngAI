using Amazon.CognitoIdentityProvider;
using EngAI.Models;
using EngAI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EngAI.Controllers;
[Route("api/lesson")]
[ApiController]
public class LessonController : ControllerBase
{
    private readonly IAmazonCognitoIdentityProvider _cognito;
    private readonly LessonService _service;
    private readonly OpenAPIService _openApiService;

    public LessonController(IAmazonCognitoIdentityProvider cognito, LessonService service, OpenAPIService openApiService)
    {
        _cognito = cognito;
        _service = service;
        _openApiService = openApiService;
    }

    // GET api/lesson/5
    [HttpGet("id={id},unitId={unitId},courseId={courseId}")]
    [Authorize]
    public async Task<ActionResult<Lesson>> GetLesson(int id, int unitId, int courseId)
    {

        var data = await _service.GetLesson(id, unitId, courseId);

        if (data == null)
        {
            return NotFound();
        }
        var response = new Lesson()
        {
            Id = id,
            Name = data.Name,
            Content = data.Type switch
            {
                Lesson.LessonType.Grammar => JsonConvert.DeserializeObject<GrammarLessonContent>(data.Content ?? ""),
                Lesson.LessonType.Vocabulary => JsonConvert.DeserializeObject<VocabularyLessonContent>(data.Content ?? ""),
                Lesson.LessonType.Conversation => JsonConvert.DeserializeObject<ConversationLessonContent>(data.Content ?? ""),
                Lesson.LessonType.Pronunciation => JsonConvert.DeserializeObject<PronunciationLessonContent>(data.Content ?? ""),
                _ => null
            },
            Type = data.Type,
        };

        return Ok(data);
    }

    // GET api/lesson/5
    [HttpGet]
    public async Task<ActionResult<Lesson>> GetCurrentLesson()
    {
        var userId = User.FindFirst("sub")?.Value;

        var data = await _service.GetCurrentLesson(userId);

        if (data == null)
        {
            return NotFound();
        }
        return Ok(data);
    }

    [HttpGet("{id}/stream")]
    public async Task<IActionResult> StreamAudio([FromQuery] string text, int id)
    {
        try
        {
            var audioStream = await _openApiService.GenerateSpeechStreamAsync(text);
            return File(audioStream, "audio/mpeg", enableRangeProcessing: true); // Optional: range for seeking
        }
        catch (Exception ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }

    // POST api/<LessonController>
    [HttpPost]
    public void Create([FromBody] string value)
    {
    }

    // PUT api/<LessonController>/5
    [HttpPut("{id}")]
    public void Update(int id, [FromBody] string value)
    {

    }

    // DELETE api/<LessonController>/5
    [HttpDelete("{id}")]
    public void Delete(int id)
    {
    }
}
