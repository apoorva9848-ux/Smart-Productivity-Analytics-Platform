using DailyTracker.API.Data;
using DailyTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyTracker.API.Repositories;

/// <summary>
/// Repository implementation for Activity data access.
/// </summary>
public class ActivityRepository : IActivityRepository
{
    private readonly ApplicationDbContext _context;

    public ActivityRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Activity>> GetByDateAsync(DateTime date)
    {
        var dateOnly = date.Date;
        return await _context.Activities
            .Include(a => a.DailyLog)
            .Where(a => a.DailyLog.Date.Date == dateOnly)
            .OrderByDescending(a => a.CreatedAt)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Activity> CreateAsync(Activity activity)
    {
        activity.CreatedAt = DateTime.UtcNow;
        _context.Activities.Add(activity);
        await _context.SaveChangesAsync();
        return activity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var activity = await _context.Activities.FindAsync(id);
        if (activity == null)
            return false;

        _context.Activities.Remove(activity);
        await _context.SaveChangesAsync();
        return true;
    }
}
