using DailyTracker.API.Models;
using DailyTracker.API.Repositories;
using System.Text.Json;

namespace DailyTracker.API.Services;

/// <summary>
/// Service implementation for AI-powered insights.
/// </summary>
public class AIService : IAIService
{
    private readonly IDailyLogRepository _dailyLogRepository;
    private readonly IDSARepository _dsaRepository;
    private readonly ITopicRepository _topicRepository;
    private readonly ILogger<AIService> _logger;

    public AIService(
        IDailyLogRepository dailyLogRepository,
        IDSARepository dsaRepository,
        ITopicRepository topicRepository,
        ILogger<AIService> logger)
    {
        _dailyLogRepository = dailyLogRepository;
        _dsaRepository = dsaRepository;
        _topicRepository = topicRepository;
        _logger = logger;
    }

    /// <summary>
    /// Generate weekly insights by collecting last 7 days of data and sending to AI.
    /// </summary>
    public async Task<AIInsightDto> GenerateWeeklyInsightAsync(AIInsightRequest request)
    {
        try
        {
            _logger.LogInformation("Generating weekly AI insights for mode: {Mode}", request.Mode);

            // Collect user activity data
            var summary = await GatherUserActivitySummaryAsync();

            // Generate AI insights based on mode
            var insight = GenerateAIInsight(summary, request.Mode);

            _logger.LogInformation("Weekly insights generated successfully");
            return insight;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating weekly insights");
            throw;
        }
    }

    /// <summary>
    /// Gather and summarize user activity from last 7 days.
    /// </summary>
    private async Task<UserActivitySummaryDto> GatherUserActivitySummaryAsync()
    {
        var today = DateTime.UtcNow.Date;
        var sevenDaysAgo = today.AddDays(-7);

        _logger.LogInformation($"Gathering activity data from {sevenDaysAgo} to {today}");

        // Fetch daily logs from last 7 days
        var dailyLogs = await _dailyLogRepository.GetByDateRangeAsync(sevenDaysAgo, today);

        // Fetch DSA problems from last 7 days
        var dsaProblems = await _dsaRepository.GetByDateRangeAsync(sevenDaysAgo, today);

        // Fetch all topics to calculate progress
        var allTopics = await _topicRepository.GetAllAsync();
        var completedTopics = await _topicRepository.GetCompletedAsync();

        // Build summary
        var summary = new UserActivitySummaryDto
        {
            StudyDaysLastWeek = dailyLogs.Count,
            DSAProblemsLastWeek = dsaProblems.Count,
            DSAByDifficulty = dsaProblems
                .GroupBy(p => p.Difficulty)
                .ToDictionary(g => g.Key, g => g.Count()),
            TopicsCompleted = completedTopics.Count,
            TotalTopics = allTopics.Count,
            TopicsByCategory = allTopics
                .GroupBy(t => t.Category)
                .ToDictionary(
                    g => g.Key,
                    g => (completed: g.Count(t => t.IsCompleted), total: g.Count())),
            RecentDailyNotes = new List<string>()
        };

        return summary;
    }

    /// <summary>
    /// Generate AI insight using mocked response with mode-based prompts.
    /// </summary>
    private AIInsightDto GenerateAIInsight(UserActivitySummaryDto summary, string mode)
    {
        var normalizedMode = string.IsNullOrWhiteSpace(mode)
            ? "normal"
            : mode.Trim().ToLowerInvariant();

        _logger.LogInformation("Generating insight from user activity summary for mode: {Mode}", normalizedMode);
        _logger.LogDebug("AI prompt generated: {Prompt}", BuildPrompt(summary, normalizedMode));

        // Calculate stats
        var isActive = summary.StudyDaysLastWeek >= 5;
        var dsaFocus = summary.DSAProblemsLastWeek >= 5;
        var topicProgress = summary.TotalTopics > 0
            ? (double)summary.TopicsCompleted / summary.TotalTopics * 100
            : 0;

        var easyCount = summary.DSAByDifficulty.GetValueOrDefault("Easy", 0);
        var mediumCount = summary.DSAByDifficulty.GetValueOrDefault("Medium", 0);
        var hardCount = summary.DSAByDifficulty.GetValueOrDefault("Hard", 0);

        var insight = new AIInsightDto();

        // Fill in stats
        insight.StudyConsistency = $"{summary.StudyDaysLastWeek}/7 days studied";
        insight.DSAStats = $"{summary.DSAProblemsLastWeek} problems solved ({easyCount}E/{mediumCount}M/{hardCount}H)";

        // Topic progress by category
        foreach (var category in summary.TopicsByCategory)
        {
            var completionRate = category.Value.total > 0
                ? (double)category.Value.completed / category.Value.total * 100
                : 0;
            insight.TopicProgress[category.Key] = $"{category.Value.completed}/{category.Value.total} ({completionRate:F0}%)";
        }

        // Generate insights based on mode
        switch (normalizedMode)
        {
            case "brutal":
                GenerateBrutalInsights(summary, insight, isActive, dsaFocus, topicProgress, easyCount, mediumCount, hardCount);
                break;
            case "motivational":
                GenerateMotivationalInsights(summary, insight, isActive, dsaFocus, topicProgress, easyCount, mediumCount, hardCount);
                break;
            default: // normal
                GenerateNormalInsights(summary, insight, isActive, dsaFocus, topicProgress, easyCount, mediumCount, hardCount);
                break;
        }

        return insight;
    }

    private string BuildPrompt(UserActivitySummaryDto summary, string mode)
    {
        var basePrompt = @"You are a brutally honest but intelligent productivity coach.

Analyze the user's weekly data:
- Study consistency
- DSA practice
- Topic progress

Your job:
1. Give a sharp, insightful summary (not generic)
2. Identify real patterns (not just numbers)
3. Point out uncomfortable truths if needed
4. Provide 2–3 high-impact actionable suggestions

Tone rules:
- Be direct
- Avoid generic advice like 'stay consistent'
- Make it feel personal and real

Example tone:
'You're not lacking time, you're lacking discipline.'
'Your effort is scattered, not focused.'";

        var modeHint = mode switch
        {
            "brutal" => "Use a very direct, no sugarcoating tone.",
            "motivational" => "Be encouraging but realistic while still naming exact next steps.",
            _ => "Keep the tone balanced: honest, insightful and focused on real patterns."
        };

        var categorySummary = summary.TopicsByCategory
            .Select(c => $"{c.Key}: {c.Value.completed}/{c.Value.total}")
            .DefaultIfEmpty("No topic data available")
            .ToArray();

        var dataBlock = $"Weekly data:\n- Study days: {summary.StudyDaysLastWeek}/7\n- DSA problems: {summary.DSAProblemsLastWeek}\n- Topics completed: {summary.TopicsCompleted}/{summary.TotalTopics}\n- Topic progress by category: {string.Join(", ", categorySummary)}";

        return $"{basePrompt}\n{modeHint}\n\n{dataBlock}";
    }

    private void GenerateNormalInsights(UserActivitySummaryDto summary, AIInsightDto insight, bool isActive, bool dsaFocus, double topicProgress, int easyCount, int mediumCount, int hardCount)
    {
        if (summary.StudyDaysLastWeek <= 4)
        {
            insight.Summary = "You’re not behind — you’re just inconsistent right now.";
            insight.Insight = $"The real pattern is clear: your effort is scattered, not focused. You studied {summary.StudyDaysLastWeek}/7 days, which makes progress feel accidental. " +
                              "This week is not about more time, it is about sharper choices and a compact routine that you can repeat.";
        }
        else
        {
            insight.Summary = "You have the foundation — now sharpen it with clearer targets.";
            insight.Insight = $"You put in {summary.StudyDaysLastWeek} study days, and that is useful. The next step is to stop treating study as a hobby and start treating it as a training block. " +
                              "Right now your biggest weakness is not effort, it is the lack of a focused DSA and topic plan.";
        }

        if (summary.DSAProblemsLastWeek == 0)
        {
            insight.Weaknesses.Add("No DSA practice this week: interview readiness needs real problem work.");
        }
        else if (summary.DSAProblemsLastWeek < 3)
        {
            insight.Weaknesses.Add("Too few DSA problems: your practice volume is still too low for technical interviews.");
        }

        if (easyCount > mediumCount + hardCount)
        {
            insight.Weaknesses.Add("Over-reliance on easy problems: you need medium/hard problems to build resilience.");
        }

        if (topicProgress < 40)
        {
            insight.Weaknesses.Add($"Low topic completion: {topicProgress:F0}% means the foundation is still fragile.");
        }

        insight.Suggestions.Add("Pick 3 target skills for next week and put them on repeat.");
        insight.Suggestions.Add($"Solve 5 DSA problems, with at least 2 at medium/hard difficulty.");
        insight.Suggestions.Add("Finish one core topic fully before moving to the next one.");
    }

    private void GenerateBrutalInsights(UserActivitySummaryDto summary, AIInsightDto insight, bool isActive, bool dsaFocus, double topicProgress, int easyCount, int mediumCount, int hardCount)
    {
        insight.Summary = "You're not lacking time, you're lacking discipline.";

        insight.Insight = $"Your weekly performance is weak, and the numbers do not lie. {summary.StudyDaysLastWeek}/7 days of study is too low, {summary.DSAProblemsLastWeek} DSA problems is not enough, and {topicProgress:F0}% topic progress leaves too many gaps. " +
                      "This is not about excuses — it is about recognizing that your current approach is not strong enough for the jobs you want.";

        insight.Weaknesses.Add($"Weak study rhythm: {summary.StudyDaysLastWeek}/7 days shows a stop-start pattern.");
        insight.Weaknesses.Add($"Shallow DSA work: {summary.DSAProblemsLastWeek} problems is not building interview toughness.");
        insight.Weaknesses.Add($"Topic gap: {topicProgress:F0}% completion is not enough to feel confident in core concepts.");

        insight.Suggestions.Add("Treat this week like a test: build a repeatable study schedule and stick to it.");
        insight.Suggestions.Add("Solve 10 DSA problems next week, with at least 3 medium/hard.");
        insight.Suggestions.Add("Close one major topic area completely before adding another.");
    }

    private void GenerateMotivationalInsights(UserActivitySummaryDto summary, AIInsightDto insight, bool isActive, bool dsaFocus, double topicProgress, int easyCount, int mediumCount, int hardCount)
    {
        insight.Summary = "You have the foundation — now turn it into stronger momentum.";

        insight.Insight = $"You're making progress, and that is the most important thing. {summary.StudyDaysLastWeek} days of study and {summary.DSAProblemsLastWeek} problems solved mean you're building habit and skill. " +
                      "The next move is to focus your energy with clearer goals, because the work you do now is what will determine how quickly you level up.";

        if (summary.DSAProblemsLastWeek < 5)
        {
            insight.Weaknesses.Add("Opportunity: More DSA practice this week will make your effort translate faster.");
        }

        if (topicProgress < 60)
        {
            insight.Weaknesses.Add("Growth area: Strengthening topic completion will make your answers more confident.");
        }

        insight.Suggestions.Add("Keep the streak: make next week a repeatable routine, not a one-off effort.");
        insight.Suggestions.Add("Add 5 problems to your DSA total, with a couple of medium/hard challenges.");
        insight.Suggestions.Add("Finish one important topic end-to-end so you can build from a stable base.");
    }
}
