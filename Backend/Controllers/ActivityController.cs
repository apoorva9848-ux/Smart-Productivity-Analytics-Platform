using DailyTracker.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DailyTracker.API.Controllers;

[ApiController]
[Route("api/activities")]
public class ActivityController : ControllerBase
{
    private readonly IActivityService _activityService;
    private readonly ILogger<ActivityController> _logger;

    public ActivityController(IActivityService activityService, ILogger<ActivityController> logger)
    {
        _activityService = activityService;
        _logger = logger;
    }

    [HttpGet("{date}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<ActivityDto>>> GetActivitiesByDate(string date)
    {
        try
        {
            if (!DateTime.TryParse(date, out var parsedDate))
                return BadRequest(new { message = "Invalid date format. Use YYYY-MM-DD." });

            _logger.LogInformation("Fetching activities for date {Date}", parsedDate.Date);
            var activities = await _activityService.GetActivitiesByDateAsync(parsedDate);
            return Ok(activities);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching activities for date {Date}", date);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error fetching activities" });
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ActivityDto>> CreateActivity([FromBody] CreateActivityRequest request)
    {
        try
        {
            if (request == null)
                return BadRequest(new { message = "Request body is required" });

            var createdActivity = await _activityService.CreateActivityAsync(request);
            var dateString = createdActivity.Date.ToString("yyyy-MM-dd");
            return CreatedAtAction(nameof(GetActivitiesByDate), new { date = dateString }, createdActivity);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error creating activity");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating activity");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error creating activity" });
        }
    }

    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteActivity(int id)
    {
        try
        {
            _logger.LogInformation("Deleting activity {ActivityId}", id);
            var result = await _activityService.DeleteActivityAsync(id);
            if (!result)
                return NotFound(new { message = $"Activity with id {id} not found" });

            return NoContent();
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid activity id {ActivityId}", id);
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting activity {ActivityId}", id);
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "Error deleting activity" });
        }
    }
}
