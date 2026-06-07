namespace DailyTracker.API.Models;

/// <summary>
/// Supported activity types for daily work tracking.
/// </summary>
public enum ActivityType
{
    DSA,
    Theory,
    Project,
    Resume,
    Other
}

/// <summary>
/// Represents an individual activity logged for a given date.
/// </summary>
public class Activity
{
    public int Id { get; set; }

    public int DailyLogId { get; set; }
    public DailyLog DailyLog { get; set; } = null!;

    public ActivityType Type { get; set; }

    public string Description { get; set; } = string.Empty;

    public int? DurationInMinutes { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
