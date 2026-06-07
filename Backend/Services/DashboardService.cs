using DailyTracker.API.Data;
using DailyTracker.API.Models;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace DailyTracker.API.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;

    public DashboardService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<HeatmapEntry>> GetHeatmapAsync(DateTime endDate, int days = 30)
    {
        var rangeStart = endDate.Date.AddDays(-(days - 1));
        var rangeEnd = endDate.Date.AddDays(1);
        var activities = await GetActivitiesForRangeAsync(rangeStart, rangeEnd);
        var dailyScores = BuildDailyScores(rangeStart, rangeEnd, activities);

        return dailyScores
            .Select(day => new HeatmapEntry
            {
                Date = day.Date.ToString("yyyy-MM-dd"),
                Score = day.Score
            })
            .ToList();
    }

    public async Task<DashboardSummaryDto> GetSummaryAsync(string month)
    {
        var monthlyScores = await GetMonthlySummaryDataAsync(month);
        return new DashboardSummaryDto
        {
            TotalScore = monthlyScores.TotalScore,
            ActiveDays = monthlyScores.ActiveDays,
            TotalActivities = monthlyScores.TotalActivities,
            DsaCount = monthlyScores.DsaCount,
            TheoryCount = monthlyScores.TheoryCount,
            ProjectCount = monthlyScores.ProjectCount,
            OtherCount = monthlyScores.OtherCount,
            MissedDays = monthlyScores.MissedDays,
            LongestInactivityStreak = monthlyScores.LongestInactivityStreak
        };
    }

    public async Task<List<TrendEntry>> GetTrendsAsync(string month)
    {
        var monthlyScores = await GetMonthlySummaryDataAsync(month);
        return monthlyScores.DailyScores
            .Select(day => new TrendEntry
            {
                Date = day.Date.ToString("yyyy-MM-dd"),
                Score = day.Score
            })
            .ToList();
    }

    public async Task<DistributionDto> GetDistributionAsync(string month)
    {
        var monthlyScores = await GetMonthlySummaryDataAsync(month);
        return new DistributionDto
        {
            Dsa = monthlyScores.DsaCount,
            Theory = monthlyScores.TheoryCount,
            Project = monthlyScores.ProjectCount,
            Resume = monthlyScores.ResumeCount,
            Other = monthlyScores.OtherCount
        };
    }

    public async Task<InsightDto> GetInsightsAsync(string month)
    {
        var monthlyScores = await GetMonthlySummaryDataAsync(month);
        var summary = monthlyScores;

        if (summary.TotalActivities == 0)
        {
            return new InsightDto
            {
                Insight = "No activities were logged this month. Start with one small task to build momentum.",
                Type = "warning"
            };
        }

        if (summary.ActiveDays < Math.Max(1, summary.DailyScores.Count / 2))
        {
            return new InsightDto
            {
                Insight = "Your consistency is low. Try to build a daily habit by logging at least one activity each day.",
                Type = "warning"
            };
        }

        if (summary.DsaCount < Math.Max(1, summary.TotalActivities / 4))
        {
            return new InsightDto
            {
                Insight = "DSA engagement is low this month. Add a few focused DSA sessions to stay interview-ready.",
                Type = "warning"
            };
        }

        if (summary.ProjectCount >= summary.DsaCount && summary.TheoryCount >= summary.DsaCount)
        {
            return new InsightDto
            {
                Insight = "Good balance across categories. Keep tracking your work and maintain the momentum.",
                Type = "success"
            };
        }

        return new InsightDto
        {
            Insight = "Your productivity pattern looks solid. Keep balancing DSA, theory, and project work.",
            Type = "info"
        };
    }

    private async Task<MonthlySummaryData> GetMonthlySummaryDataAsync(string month)
    {
        if (!DateTime.TryParseExact(month + "-01", "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out var monthStart))
        {
            throw new ArgumentException("Invalid month format. Use YYYY-MM.");
        }

        var monthEnd = monthStart.AddMonths(1);
        var activities = await GetActivitiesForRangeAsync(monthStart, monthEnd);
        var dailyScores = BuildDailyScores(monthStart, monthEnd, activities);
        var totalActivities = activities.Count;
        var dsaCount = activities.Count(a => a.Type == ActivityType.DSA);
        var theoryCount = activities.Count(a => a.Type == ActivityType.Theory);
        var projectCount = activities.Count(a => a.Type == ActivityType.Project);
        var resumeCount = activities.Count(a => a.Type == ActivityType.Resume);
        var otherCount = activities.Count(a => a.Type == ActivityType.Other);
        var totalScore = dailyScores.Sum(d => d.Score);
        var activeDays = dailyScores.Count(d => d.Score > 0);
        var missedDays = dailyScores.Count(d => d.Score == 0);
        var longestStreak = CalculateLongestStreak(dailyScores);

        return new MonthlySummaryData
        {
            TotalScore = totalScore,
            ActiveDays = activeDays,
            TotalActivities = totalActivities,
            DsaCount = dsaCount,
            TheoryCount = theoryCount,
            ProjectCount = projectCount,
            ResumeCount = resumeCount,
            OtherCount = otherCount,
            MissedDays = missedDays,
            LongestInactivityStreak = longestStreak,
            DailyScores = dailyScores
        };
    }

    private async Task<List<Activity>> GetActivitiesForRangeAsync(DateTime rangeStart, DateTime rangeEnd)
    {
        return await _context.Activities
            .Include(a => a.DailyLog)
            .Where(a => a.DailyLog.Date >= rangeStart && a.DailyLog.Date < rangeEnd)
            .ToListAsync();
    }

    private List<DailyScore> BuildDailyScores(DateTime rangeStart, DateTime rangeEnd, List<Activity> activities)
    {
        var groupedScores = activities
            .GroupBy(a => a.DailyLog.Date.Date)
            .ToDictionary(g => g.Key, g => g.Sum(a => GetActivityScore(a.Type)));

        var result = new List<DailyScore>();
        var current = rangeStart.Date;

        while (current < rangeEnd.Date)
        {
            groupedScores.TryGetValue(current, out var score);
            result.Add(new DailyScore
            {
                Date = current,
                Score = score
            });
            current = current.AddDays(1);
        }

        return result;
    }

    private int GetActivityScore(ActivityType activityType)
    {
        return activityType switch
        {
            ActivityType.DSA => 2,
            ActivityType.Project => 2,
            ActivityType.Theory => 1,
            ActivityType.Other => 1,
            _ => 1
        };
    }

    private int CalculateLongestStreak(List<DailyScore> dailyScores)
    {
        var longest = 0;
        var currentStreak = 0;

        foreach (var entry in dailyScores)
        {
            if (entry.Score == 0)
            {
                currentStreak++;
                longest = Math.Max(longest, currentStreak);
            }
            else
            {
                currentStreak = 0;
            }
        }

        return longest;
    }

    private class MonthlySummaryData
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
        public List<DailyScore> DailyScores { get; set; } = new();
    }

    private class DailyScore
    {
        public DateTime Date { get; set; }
        public int Score { get; set; }
    }
}
