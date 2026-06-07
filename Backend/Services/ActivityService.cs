using DailyTracker.API.Models;
using DailyTracker.API.Repositories;

namespace DailyTracker.API.Services;

/// <summary>
/// Service implementation for activity business logic.
/// </summary>
public class ActivityService : IActivityService
{
    private readonly IActivityRepository _activityRepository;
    private readonly IDailyLogRepository _dailyLogRepository;

    public ActivityService(IActivityRepository activityRepository, IDailyLogRepository dailyLogRepository)
    {
        _activityRepository = activityRepository;
        _dailyLogRepository = dailyLogRepository;
    }

    public async Task<List<ActivityDto>> GetActivitiesByDateAsync(DateTime date)
    {
        var activities = await _activityRepository.GetByDateAsync(date);
        return activities.Select(MapToDto).ToList();
    }

    public async Task<ActivityDto> CreateActivityAsync(CreateActivityRequest request)
    {
        if (request == null)
            throw new ArgumentNullException(nameof(request));

        if (request.Date == default)
            throw new ArgumentException("A valid date is required.");

        if (string.IsNullOrWhiteSpace(request.Description))
            throw new ArgumentException("Description is required.");

        var dateOnly = request.Date.Date;
        var dailyLog = await _dailyLogRepository.GetByDateAsync(dateOnly);
        if (dailyLog == null)
        {
            dailyLog = await _dailyLogRepository.CreateAsync(new DailyLog
            {
                Date = dateOnly,
                CreatedAt = DateTime.UtcNow
            });
        }

        var activity = new Activity
        {
            DailyLogId = dailyLog.Id,
            Type = request.Type,
            Description = request.Description.Trim(),
            DurationInMinutes = request.DurationInMinutes,
            CreatedAt = DateTime.UtcNow
        };

        var createdActivity = await _activityRepository.CreateAsync(activity);
        createdActivity.DailyLog = dailyLog; // Attach the DailyLog for DTO mapping
        return MapToDto(createdActivity);
    }

    public async Task<bool> DeleteActivityAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("Invalid activity id.");

        return await _activityRepository.DeleteAsync(id);
    }

    private static ActivityDto MapToDto(Activity activity)
    {
        return new ActivityDto
        {
            Id = activity.Id,
            DailyLogId = activity.DailyLogId,
            Date = activity.DailyLog?.Date ?? DateTime.MinValue,
            Type = activity.Type,
            Description = activity.Description,
            DurationInMinutes = activity.DurationInMinutes,
            CreatedAt = activity.CreatedAt
        };
    }
}
