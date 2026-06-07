using System.Globalization;
using System.Text.Json.Serialization;

namespace DailyTracker.API.Services;

public class HeatmapEntry
{
    public string Date { get; set; } = string.Empty;
    public int Score { get; set; }
}

public class DashboardSummaryDto
{
    public int TotalScore { get; set; }
    public int ActiveDays { get; set; }
    public int TotalActivities { get; set; }
    public int DsaCount { get; set; }
    public int TheoryCount { get; set; }
    public int ProjectCount { get; set; }
    public int ResumeCount { get; set; }
    public int OtherCount { get; set; }
    public int MissedDays { get; set; }
    public int LongestInactivityStreak { get; set; }
}

public class TrendEntry
{
    public string Date { get; set; } = string.Empty;
    public int Score { get; set; }
}

public class DistributionDto
{
    public int Dsa { get; set; }
    public int Theory { get; set; }
    public int Project { get; set; }
    public int Resume { get; set; }
    public int Other { get; set; }
}

public class InsightDto
{
    public string Insight { get; set; } = string.Empty;
    public string Type { get; set; } = "info";
}

public interface IDashboardService
{
    Task<List<HeatmapEntry>> GetHeatmapAsync(DateTime endDate, int days = 30);
    Task<DashboardSummaryDto> GetSummaryAsync(string month);
    Task<List<TrendEntry>> GetTrendsAsync(string month);
    Task<DistributionDto> GetDistributionAsync(string month);
    Task<InsightDto> GetInsightsAsync(string month);
}
