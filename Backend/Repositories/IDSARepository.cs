using DailyTracker.API.Models;

namespace DailyTracker.API.Repositories;

/// <summary>
/// Repository interface for DSAProblem data access operations.
/// </summary>
public interface IDSARepository
{
    /// <summary>
    /// Get all DSA problems.
    /// </summary>
    Task<List<DSAProblem>> GetAllAsync();

    /// <summary>
    /// Get a DSA problem by ID.
    /// </summary>
    Task<DSAProblem?> GetByIdAsync(int id);

    /// <summary>
    /// Get DSA problems filtered by difficulty.
    /// </summary>
    Task<List<DSAProblem>> GetByDifficultyAsync(string difficulty);

    /// <summary>
    /// Get DSA problems filtered by pattern.
    /// </summary>
    Task<List<DSAProblem>> GetByPatternAsync(string pattern);

    /// <summary>
    /// Get DSA problems by solved date range.
    /// </summary>
    Task<List<DSAProblem>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Create a new DSA problem entry.
    /// </summary>
    Task<DSAProblem> CreateAsync(DSAProblem problem);

    /// <summary>
    /// Update an existing DSA problem.
    /// </summary>
    Task<DSAProblem> UpdateAsync(DSAProblem problem);

    /// <summary>
    /// Delete a DSA problem.
    /// </summary>
    Task<bool> DeleteAsync(int id);
}
