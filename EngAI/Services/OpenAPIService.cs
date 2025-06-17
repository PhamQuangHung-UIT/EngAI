using Newtonsoft.Json;
using System.Text;

namespace EngAI.Services;

public class OpenAPIService(IConfiguration config)
{
    private readonly IConfiguration _configuration = config;
    private readonly HttpClient _httpClient = new();

    private readonly string GenerateCourseAssistantId = config["OpenAPI:GenerateCourseAssistantId"];
    private readonly string PronunciationAssistantId = config["PronunciationAssistantId"];
    private readonly string VocabularyAssistantId = config["VocabularyAssistantId"];
    private readonly string GrammarAssistantId;
    private readonly string ConversationAssistantId;

    public async Task<Stream> GenerateSpeechStreamAsync(string inputText)
    {
        var apiKey = _configuration["OpenAI:ApiKey"];

        if (string.IsNullOrWhiteSpace(apiKey))
            throw new InvalidOperationException("OpenAI API key is missing.");

        var requestUri = "https://api.openai.com/v1/audio/speech";

        var requestBody = new
        {
            model = "gpt-4o-mini-tts",
            input = inputText,
            voice = "alloy"
        };

        var json = JsonConvert.SerializeObject(requestBody);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
        {
            Content = content
        };
        request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", apiKey);

        var response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"OpenAI API call failed: {response.StatusCode}, {error}");
        }

        // Return the raw audio stream to the controller
        return await response.Content.ReadAsStreamAsync();
    }
}
