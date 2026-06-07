using DailyTracker.API.Models;

namespace DailyTracker.API.Services;

/// <summary>
/// DTOs for DSA Problem API requests and responses.
/// </summary>
public class CreateDSAProblemRequest
{
    public string Name { get; set; } = string.Empty;
    public DateTime DateSolved { get; set; }
    public string? Link { get; set; }
    public string Difficulty { get; set; } = string.Empty; // Easy, Medium, Hard
    public string? Pattern { get; set; }
    public string? Notes { get; set; }
}

public class UpdateDSAProblemRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateSolved { get; set; }
    public string? Link { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public string? Pattern { get; set; }
    public string? Notes { get; set; }
}

public class DSAProblemDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DateSolved { get; set; }
    public string? Link { get; set; }
    public string Difficulty { get; set; } = string.Empty;
    public string? Pattern { get; set; }
    public string? Notes { get; set; }
}

/// <summary>
/// Service interface for DSA problem business logic.
/// </summary>
public interface IDSAService
{
    /// <summary>
    /// Get all DSA problems.
    /// </summary>
    Task<List<DSAProblemDto>> GetAllProblemsAsync();

    /// <summary>
    /// Get a DSA problem by ID.
    /// </summary>
    Task<DSAProblemDto?> GetProblemByIdAsync(int id);

    /// <summary>
    /// Get DSA problems by difficulty level.
    /// </summary>
    Task<List<DSAProblemDto>> GetProblemsByDifficultyAsync(string difficulty);

    /// <summary>
    /// Get DSA problems by pattern.
    /// </summary>
    Task<List<DSAProblemDto>> GetProblemsByPatternAsync(string pattern);

    /// <summary>
    /// Get DSA problems solved within a date range.
    /// </summary>
    Task<List<DSAProblemDto>> GetProblemsByDateRangeAsync(DateTime startDate, DateTime endDate);

    /// <summary>
    /// Create a new DSA problem entry.
    /// </summary>
    Task<DSAProblemDto> CreateProblemAsync(CreateDSAProblemRequest request);

    /// <summary>
    /// Update an existing DSA problem.
    /// </summary>
    Task<DSAProblemDto> UpdateProblemAsync(UpdateDSAProblemRequest request);

    /// <summary>
    /// Delete a DSA problem.
    /// </summary>
    Task<bool> DeleteProblemAsync(int id);
}
