using DailyTracker.API.Models;
using DailyTracker.API.Repositories;

namespace DailyTracker.API.Services;

/// <summary>
/// Service implementation for DSA problem business logic.
/// </summary>
public class DSAService : IDSAService
{
    private readonly IDSARepository _repository;

    public DSAService(IDSARepository repository)
    {
        _repository = repository;
    }

    /// <summary>
    /// Get all DSA problems and convert to DTOs.
    /// </summary>
    public async Task<List<DSAProblemDto>> GetAllProblemsAsync()
    {
        var problems = await _repository.GetAllAsync();
        return problems.Select(MapToDto).ToList();
    }

    /// <summary>
    /// Get a DSA problem by ID and convert to DTO.
    /// </summary>
    public async Task<DSAProblemDto?> GetProblemByIdAsync(int id)
    {
        var problem = await _repository.GetByIdAsync(id);
        return problem != null ? MapToDto(problem) : null;
    }

    /// <summary>
    /// Get DSA problems by difficulty level.
    /// </summary>
    public async Task<List<DSAProblemDto>> GetProblemsByDifficultyAsync(string difficulty)
    {
        ValidateDifficulty(difficulty);
        var problems = await _repository.GetByDifficultyAsync(difficulty);
        return problems.Select(MapToDto).ToList();
    }

    /// <summary>
    /// Get DSA problems by pattern.
    /// </summary>
    public async Task<List<DSAProblemDto>> GetProblemsByPatternAsync(string pattern)
    {
        if (string.IsNullOrWhiteSpace(pattern))
            throw new ArgumentException("Pattern cannot be empty");

        var problems = await _repository.GetByPatternAsync(pattern);
        return problems.Select(MapToDto).ToList();
    }

    /// <summary>
    /// Get DSA problems solved within a date range.
    /// </summary>
    public async Task<List<DSAProblemDto>> GetProblemsByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var problems = await _repository.GetByDateRangeAsync(startDate, endDate);
        return problems.Select(MapToDto).ToList();
    }

    /// <summary>
    /// Create a new DSA problem entry from request.
    /// </summary>
    public async Task<DSAProblemDto> CreateProblemAsync(CreateDSAProblemRequest request)
    {
        // Validate request
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Problem name is required");

        ValidateDifficulty(request.Difficulty);

        var problem = new DSAProblem
        {
            Name = request.Name.Trim(),
            DateSolved = request.DateSolved,
            Link = request.Link?.Trim(),
            Difficulty = request.Difficulty.Trim(),
            Pattern = request.Pattern?.Trim(),
            Notes = request.Notes?.Trim()
        };

        var createdProblem = await _repository.CreateAsync(problem);
        return MapToDto(createdProblem);
    }

    /// <summary>
    /// Update an existing DSA problem.
    /// </summary>
    public async Task<DSAProblemDto> UpdateProblemAsync(UpdateDSAProblemRequest request)
    {
        // Get existing problem
        var problem = await _repository.GetByIdAsync(request.Id);
        if (problem == null)
            throw new ArgumentException($"DSA problem with ID {request.Id} not found");

        // Validate request
        if (string.IsNullOrWhiteSpace(request.Name))
            throw new ArgumentException("Problem name is required");

        ValidateDifficulty(request.Difficulty);

        // Update fields
        problem.Name = request.Name.Trim();
        problem.DateSolved = request.DateSolved;
        problem.Link = request.Link?.Trim();
        problem.Difficulty = request.Difficulty.Trim();
        problem.Pattern = request.Pattern?.Trim();
        problem.Notes = request.Notes?.Trim();

        var updatedProblem = await _repository.UpdateAsync(problem);
        return MapToDto(updatedProblem);
    }

    /// <summary>
    /// Delete a DSA problem by ID.
    /// </summary>
    public async Task<bool> DeleteProblemAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    /// <summary>
    /// Validate difficulty level.
    /// </summary>
    private static void ValidateDifficulty(string difficulty)
    {
        var validDifficulties = new[] { "Easy", "Medium", "Hard" };
        if (!validDifficulties.Contains(difficulty, StringComparer.OrdinalIgnoreCase))
            throw new ArgumentException("Difficulty must be Easy, Medium, or Hard");
    }

    /// <summary>
    /// Map DSAProblem entity to DTO.
    /// </summary>
    private static DSAProblemDto MapToDto(DSAProblem problem)
    {
        return new DSAProblemDto
        {
            Id = problem.Id,
            Name = problem.Name,
            DateSolved = problem.DateSolved,
            Link = problem.Link,
            Difficulty = problem.Difficulty,
            Pattern = problem.Pattern,
            Notes = problem.Notes
        };
    }
}
