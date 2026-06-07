using DailyTracker.API.Data;
using DailyTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyTracker.API.Repositories;

/// <summary>
/// Repository implementation for DailyLog data access operations.
/// </summary>
public class DailyLogRepository : IDailyLogRepository
{
    private readonly ApplicationDbContext _context;

    public DailyLogRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all daily logs ordered by date descending.
    /// </summary>
    public async Task<List<DailyLog>> GetAllAsync()
    {
        return await _context.DailyLogs
            .OrderByDescending(x => x.Date)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Get a daily log by ID.
    /// </summary>
    public async Task<DailyLog?> GetByIdAsync(int id)
    {
        return await _context.DailyLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Get daily log by date (only one per day).
    /// </summary>
    public async Task<DailyLog?> GetByDateAsync(DateTime date)
    {
        var dateOnly = date.Date; // Get only the date part
        return await _context.DailyLogs
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Date.Date == dateOnly);
    }

    /// <summary>
    /// Get daily logs within a date range.
    /// </summary>
    public async Task<List<DailyLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.DailyLogs
            .Where(x => x.Date.Date >= startDate.Date && x.Date.Date <= endDate.Date)
            .OrderByDescending(x => x.Date)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Create a new daily log.
    /// </summary>
    public async Task<DailyLog> CreateAsync(DailyLog dailyLog)
    {
        dailyLog.CreatedAt = DateTime.UtcNow;
        _context.DailyLogs.Add(dailyLog);
        await _context.SaveChangesAsync();
        return dailyLog;
    }

    /// <summary>
    /// Update an existing daily log.
    /// </summary>
    public async Task<DailyLog> UpdateAsync(DailyLog dailyLog)
    {
        dailyLog.UpdatedAt = DateTime.UtcNow;
        _context.DailyLogs.Update(dailyLog);
        await _context.SaveChangesAsync();
        return dailyLog;
    }

    /// <summary>
    /// Delete a daily log by ID.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var log = await _context.DailyLogs.FindAsync(id);
        if (log == null)
            return false;

        _context.DailyLogs.Remove(log);
        await _context.SaveChangesAsync();
        return true;
    }
}
