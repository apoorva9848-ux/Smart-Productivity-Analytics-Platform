using DailyTracker.API.Models;

namespace DailyTracker.API.Repositories;

/// <summary>
/// Repository interface for DailyLog data access operations.
/// </summary>
public interface IDailyLogRepository
{
    /// <summary>
    /// Get all daily logs.
    /// </summary>
    Task<List<DailyLog>> GetAllAsync();

    /// <summary>
    /// Get a daily log by ID.
    /// </summary>
    Task<DailyLog?> GetByIdAsync(int id);

    /// <summary>
    /// Get daily log by date.
    /// </summary>
    Task<DailyLog?> GetByDateAsync(DateTime date);

    /// <summary>
    /// Get daily logs within a date range.
    /// </summary>
    Task<List<DailyLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Create a new daily log.
    /// </summary>
    Task<DailyLog> CreateAsync(DailyLog dailyLog);

    /// <summary>
    /// Update an existing daily log.
    /// </summary>
    Task<DailyLog> UpdateAsync(DailyLog dailyLog);

    /// <summary>
    /// Delete a daily log.
    /// </summary>
    Task<bool> DeleteAsync(int id);
}
