namespace DailyTracker.API.Models;

/// <summary>
/// Represents a topic in the full-stack preparation tracker.
/// </summary>
public class Topic
{
    /// <summary>
    /// Unique identifier for the topic.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Name of the topic.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Category of the topic (Angular, .NET, SQL).
    /// </summary>
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// Whether the topic is completed.
    /// </summary>
    public bool IsCompleted { get; set; } = false;

    /// <summary>
    /// Date when the topic was completed.
    /// </summary>
    public DateTime? DateCompleted { get; set; }

    /// <summary>
    /// Additional notes for the topic.
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