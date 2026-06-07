namespace DailyTracker.API.Models;

/// <summary>
/// Represents a DSA problem tracking entry.
/// </summary>
public class DSAProblem
{
    /// <summary>
    /// Unique identifier for the DSA problem.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name or title of the problem.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Date when the problem was solved.
    /// </summary>
    public DateTime DateSolved { get; set; }

    /// <summary>
    /// Link to the problem (e.g., LeetCode URL).
    /// </summary>
    public string? Link { get; set; }

    /// <summary>
    /// Difficulty level of the problem.
    /// Easy / Medium / Hard
    /// </summary>
    public string Difficulty { get; set; } = string.Empty;

    /// <summary>
    /// Pattern or category (e.g., Hashing, Sliding Window, DFS, etc.).
    /// </summary>
    public string? Pattern { get; set; }

    /// <summary>
    /// Additional notes about the problem solution.
    /// </summary>
    public string? Notes { get; set; }

    /// <summary>
    /// Timestamp when the record was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Timestamp when the record was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }
}
