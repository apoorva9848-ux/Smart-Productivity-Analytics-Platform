using DailyTracker.API.Data;
using DailyTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyTracker.API.Repositories;

/// <summary>
/// Repository implementation for DSAProblem data access operations.
/// </summary>
public class DSARepository : IDSARepository
{
    private readonly ApplicationDbContext _context;

    public DSARepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all DSA problems ordered by date solved descending.
    /// </summary>
    public async Task<List<DSAProblem>> GetAllAsync()
    {
        return await _context.DSAProblems
            .OrderByDescending(x => x.DateSolved)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Get a DSA problem by ID.
    /// </summary>
    public async Task<DSAProblem?> GetByIdAsync(int id)
    {
        return await _context.DSAProblems
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    /// <summary>
    /// Get DSA problems filtered by difficulty.
    /// </summary>
    public async Task<List<DSAProblem>> GetByDifficultyAsync(string difficulty)
    {
        return await _context.DSAProblems
            .Where(x => x.Difficulty.ToLower() == difficulty.ToLower())
            .OrderByDescending(x => x.DateSolved)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Get DSA problems filtered by pattern.
    /// </summary>
    public async Task<List<DSAProblem>> GetByPatternAsync(string pattern)
    {
        return await _context.DSAProblems
            .Where(x => x.Pattern != null && x.Pattern.ToLower().Contains(pattern.ToLower()))
            .OrderByDescending(x => x.DateSolved)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Get DSA problems by solved date range.
    /// </summary>
    public async Task<List<DSAProblem>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.DSAProblems
            .Where(x => x.DateSolved.Date >= startDate.Date && x.DateSolved.Date <= endDate.Date)
            .OrderByDescending(x => x.DateSolved)
            .AsNoTracking()
            .ToListAsync();
    }

    /// <summary>
    /// Create a new DSA problem entry.
    /// </summary>
    public async Task<DSAProblem> CreateAsync(DSAProblem problem)
    {
        problem.CreatedAt = DateTime.UtcNow;
        _context.DSAProblems.Add(problem);
        await _context.SaveChangesAsync();
        return problem;
    }

    /// <summary>
    /// Update an existing DSA problem.
    /// </summary>
    public async Task<DSAProblem> UpdateAsync(DSAProblem problem)
    {
        problem.UpdatedAt = DateTime.UtcNow;
        _context.DSAProblems.Update(problem);
        await _context.SaveChangesAsync();
        return problem;
    }

    /// <summary>
    /// Delete a DSA problem by ID.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var problem = await _context.DSAProblems.FindAsync(id);
        if (problem == null)
            return false;

        _context.DSAProblems.Remove(problem);
        await _context.SaveChangesAsync();
        return true;
    }
}
