using Dapper;
using EngAI.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Data;

namespace EngAI.Services;

public class LessonService(IDbConnection db, IDistributedCache cache)
{
    public IDbConnection _db = db;
    public IDistributedCache _cache = cache;

    // Add methods for lesson management here, such as GetLesson, CreateLesson, UpdateLesson, DeleteLesson, etc.
    // Example:
    public async Task<LessonDTO?> GetLesson(int id, int unitId, int courseId)
    {
        const string query = "SELECT * FROM lessons WHERE id = @Id and unit_id = @UnitId and course_id = @CourseId";

        var queryResult = await _db.QueryAsync<LessonDTO>(query, new
        {
            Id = id,
            UnitId = unitId,
            CourseId = courseId
        });
        return queryResult.FirstOrDefault();
    }

    public async Task SaveCurrentLesson(string userId, Lesson lesson)
    {
        var dataString = JsonConvert.SerializeObject(lesson);
        await _cache.SetStringAsync(userId, dataString);
    }

    public async Task<Lesson?> GetCurrentLesson(string userId)
    {

        var dataString = await _cache.GetStringAsync(userId);

        if (dataString != null)
        {
            return JsonConvert.DeserializeObject<Lesson>(dataString);
        }
        return null;
    }
}
