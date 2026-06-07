namespace DailyTracker.API.Models;

/// <summary>
/// Represents a daily log date entry for activities.
/// </summary>
public class DailyLog
{
    /// <summary>
    /// Unique identifier for the daily log.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Date of the log entry.
    /// </summary>
    public DateTime Date { get; set; }

    /// <summary>
    /// Timestamp when the daily log was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Timestamp when the daily log was last updated.
    /// </summary>
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Activities logged for this date.
    /// </summary>
    public List<Activity> Activities { get; set; } = new();
}
