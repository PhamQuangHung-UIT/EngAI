using Dapper;
using EngAI.Models;
using Google.Apis.Auth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Net.Http.Headers;

namespace EngAI.Services;

public class UserService(IDbConnection db, ILogger logger)
{
    public IDbConnection _db = db;
    public ILogger _logger = logger;

    public async Task<UserDTO> AddUser(User user)
    {
        const string upsertQuery = @"
            INSERT INTO users (name, email, avatar_url)
            VALUES (@Name, @Email, @AvatarUrl)
            ON CONFLICT (email) DO UPDATE
            SET name = EXCLUDED.name, avatar_url = EXCLUDED.avatar_url
            RETURNING user_id AS Id, name AS Name, email AS Email, avatar_url AS AvatarUrl";

        try
        {
            // Insert or update the user and return the user details
            var userResult = await _db.QueryFirstAsync<UserDTO>(upsertQuery, new
            {
                user.Name,
                user.Email,
                user.AvatarUrl
            });

            return userResult;
        }
        catch (Exception ex)
        {
            // Log the exception (if logging is implemented) and rethrow or handle it as needed
            throw new Exception("An error occurred while adding the user.", ex);
        }
    }

    public async Task<User> GetGoogleUserInfoAsync(string accessToken)
    {
        var payload = await GoogleJsonWebSignature.ValidateAsync(accessToken);

        return new User
        {
            Email = payload.Email,
            Name = payload.Name,
            AvatarUrl = payload.Picture,
        };
    }

    public async Task<User> GetFacebookUserInfoAsync(string accessToken)
    {
        using var httpClient = new HttpClient();
        var url = $"https://graph.facebook.com/me?fields=id,name,email,picture.type(large)&access_token={accessToken}";
        var response = await httpClient.GetAsync(url);

        if (!response.IsSuccessStatusCode)
            throw new Exception("Facebook token verification failed");

        var content = await response.Content.ReadAsStringAsync();
        var json = JsonConvert.DeserializeObject<JObject>(content);

        return new User
        {
            Name = json.GetValue("name")!.ToString(),
            Email = json.GetValue("email") is null ? json.GetValue("email")!.ToString() : $"{json.GetValue("id")}@facebook.com",
            AvatarUrl = json.SelectToken("picture.data.url")!.ToString()
        };
    }
}
