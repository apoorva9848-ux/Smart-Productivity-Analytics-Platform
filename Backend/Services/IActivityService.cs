using DailyTracker.API.Models;
using System.Text.Json.Serialization;

namespace DailyTracker.API.Services;

/// <summary>
/// DTO used to create an activity.
/// </summary>
public class CreateActivityRequest
{
    public DateTime Date { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ActivityType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public int? DurationInMinutes { get; set; }
}

/// <summary>
/// DTO returned by activity endpoints.
/// </summary>
public class ActivityDto
{
    public int Id { get; set; }
    public int DailyLogId { get; set; }
    public DateTime Date { get; set; }
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ActivityType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public int? DurationInMinutes { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// Service interface for activity business logic.
/// </summary>
public interface IActivityService
{
    Task<List<ActivityDto>> GetActivitiesByDateAsync(DateTime date);
    Task<ActivityDto> CreateActivityAsync(CreateActivityRequest request);
    Task<bool> DeleteActivityAsync(int id);
}
