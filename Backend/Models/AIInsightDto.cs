namespace DailyTracker.API.Models;

/// <summary>
/// DTO for AI insight response.
/// </summary>
public class AIInsightDto
{
    /// <summary>
    /// Main insight about the user's productivity patterns.
    /// </summary>
    public string Insight { get; set; } = string.Empty;

    /// <summary>
    /// List of identified weaknesses.
    /// </summary>
    public List<string> Weaknesses { get; set; } = new();

    /// <summary>
    /// List of actionable suggestions.
    /// </summary>
    public List<string> Suggestions { get; set; } = new();

    /// <summary>
    /// One-line summary for quick overview.
    /// </summary>
    public string Summary { get; set; } = string.Empty;

    /// <summary>
    /// Study consistency stats.
    /// </summary>
    public string StudyConsistency { get; set; } = string.Empty;

    /// <summary>
    /// DSA problems solved.
    /// </summary>
    public string DSAStats { get; set; } = string.Empty;

    /// <summary>
    /// Topic progress by category.
    /// </summary>
    public Dictionary<string, string> TopicProgress { get; set; } = new();
}

/// <summary>
/// DTO for AI insight request.
/// </summary>
public class AIInsightRequest
{
    /// <summary>
    /// Mode for AI analysis: normal, brutal, or motivational.
    /// </summary>
    public string Mode { get; set; } = "normal";
}

/// <summary>
/// DTO for summarized user data sent to AI.
/// </summary>
public class UserActivitySummaryDto
{
    /// <summary>
    /// Number of days user studied in last 7 days.
    /// </summary>
    public int StudyDaysLastWeek { get; set; }

    /// <summary>
    /// Total DSA problems solved in last 7 days.
    /// </summary>
    public int DSAProblemsLastWeek { get; set; }

    /// <summary>
    /// Distribution of DSA problems by difficulty.
    /// </summary>
    public Dictionary<string, int> DSAByDifficulty { get; set; } = new();

    /// <summary>
    /// Total topics completed.
    /// </summary>
    public int TopicsCompleted { get; set; }

    /// <summary>
    /// Total topics in system.
    /// </summary>
    public int TotalTopics { get; set; }

    /// <summary>
    /// Topics completed by category.
    /// </summary>
    public Dictionary<string, (int completed, int total)> TopicsByCategory { get; set; } = new();

    /// <summary>
    /// Recent daily study summary.
    /// </summary>
    public List<string> RecentDailyNotes { get; set; } = new();
}
