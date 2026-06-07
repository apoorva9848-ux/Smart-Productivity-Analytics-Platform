namespace DailyTracker.API.Services;

using DailyTracker.API.Models;

/// <summary>
/// Service interface for AI-powered insights.
/// </summary>
public interface IAIService
{
    /// <summary>
    /// Generate weekly insights based on last 7 days of user activity.
    /// </summary>
    Task<AIInsightDto> GenerateWeeklyInsightAsync(AIInsightRequest request);
}
