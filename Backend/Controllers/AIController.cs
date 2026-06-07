using DailyTracker.API.Services;
using DailyTracker.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace DailyTracker.API.Controllers;

/// <summary>
/// API controller for AI-powered insights.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AIController : ControllerBase
{
    private readonly IAIService _aiService;
    private readonly ILogger<AIController> _logger;

    public AIController(IAIService aiService, ILogger<AIController> logger)
    {
        _aiService = aiService;
        _logger = logger;
    }

    /// <summary>
    /// POST /api/ai/insights
    /// Generate weekly AI insights based on last 7 days of user activity.
    /// </summary>
    [HttpPost("insights")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<AIInsightDto>> GenerateWeeklyInsights([FromBody] AIInsightRequest request)
    {
        try
        {
            if (request == null)
            {
                return BadRequest(new { message = "Request body is required" });
            }

            _logger.LogInformation("Processing request for weekly AI insights with mode: {Mode}", request.Mode);
            var insight = await _aiService.GenerateWeeklyInsightAsync(request);
            return Ok(insight);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating weekly insights");
            return StatusCode(
                StatusCodes.Status500InternalServerError,
                new { message = "An error occurred while generating insights", error = ex.Message }
            );
        }
    }
}
