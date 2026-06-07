using DailyTracker.API.Models;

namespace DailyTracker.API.Repositories;

/// <summary>
/// Repository interface for managing Activity data.
/// </summary>
public interface IActivityRepository
{
    Task<List<Activity>> GetByDateAsync(DateTime date);
    Task<Activity> CreateAsync(Activity activity);
    Task<bool> DeleteAsync(int id);
}
