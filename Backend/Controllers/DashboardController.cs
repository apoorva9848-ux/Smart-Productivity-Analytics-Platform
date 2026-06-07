using DailyTracker.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DailyTracker.API.Controllers;

[ApiController]
[Route("api/dashboard")]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("heatmap")]
    public async Task<ActionResult<List<HeatmapEntry>>> GetHeatmap()
    {
        var heatmap = await _dashboardService.GetHeatmapAsync(DateTime.UtcNow.Date);
        return Ok(heatmap);
    }

    [HttpGet("summary")]
    public async Task<ActionResult<DashboardSummaryDto>> GetSummary([FromQuery] string? month)
    {
        month = GetMonthOrDefault(month);

        try
        {
            var summary = await _dashboardService.GetSummaryAsync(month);
            return Ok(summary);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("trends")]
    public async Task<ActionResult<List<TrendEntry>>> GetTrends([FromQuery] string? month)
    {
        month = GetMonthOrDefault(month);

        try
        {
            var trends = await _dashboardService.GetTrendsAsync(month);
            return Ok(trends);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("distribution")]
    public async Task<ActionResult<DistributionDto>> GetDistribution([FromQuery] string? month)
    {
        month = GetMonthOrDefault(month);

        try
        {
            var distribution = await _dashboardService.GetDistributionAsync(month);
            return Ok(distribution);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet("insights")]
    public async Task<ActionResult<InsightDto>> GetInsights([FromQuery] string? month)
    {
        month = GetMonthOrDefault(month);

        try
        {
            var insight = await _dashboardService.GetInsightsAsync(month);
            return Ok(insight);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    private static string GetMonthOrDefault(string month)
    {
        if (!string.IsNullOrWhiteSpace(month))
        {
            return month;
        }

        return DateTime.UtcNow.ToString("yyyy-MM");
    }
}
