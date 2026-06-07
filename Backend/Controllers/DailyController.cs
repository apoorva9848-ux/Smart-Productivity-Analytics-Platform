using DailyTracker.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DailyTracker.API.Controllers;

/// <summary>
/// API controller for daily log operations.
/// </summary>
// Support both the legacy dailylogs route and the documented /api/daily contract.
[ApiController]
[Route("api/daily")]
[Route("api/dailylogs")]
public class DailyController : ControllerBase
{
    private readonly IDailyLogService _dailyLogService;
    private readonly ILogger<DailyController> _logger;

    public DailyController(IDailyLogService dailyLogService, ILogger<DailyController> logger)
    {
        _dailyLogService = dailyLogService;
        _logger = logger;
    }

    /// <summary>
    /// GET /api/daily
    /// Get all daily logs.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DailyLogDto>>> GetAllDailyLogs()
    {
        try
        {
            _logger.LogInformation("Fetching all daily logs");
            var logs = await _dailyLogService.GetAllDailyLogsAsync();
            return Ok(logs);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching daily logs");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Error fetching daily logs" });
        }
    }

    /// <summary>
    /// GET /api/daily/{id}
    /// Get a specific daily log by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DailyLogDto>> GetDailyLogById(int id)
    {
        try
        {
            _logger.LogInformation($"Fetching daily log with ID {id}");
            var log = await _dailyLogService.GetDailyLogByIdAsync(id);

            if (log == null)
                return NotFound(new { message = $"Daily log with ID {id} not found" });

            return Ok(log);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching daily log with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Error fetching daily log" });
        }
    }

    /// <summary>
    /// POST /api/daily
    /// Create a new daily log.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<DailyLogDto>> CreateDailyLog([FromBody] CreateDailyLogRequest request)
    {
        try
        {
            _logger.LogInformation($"Creating daily log for date {request.Date:yyyy-MM-dd}");

            // Validate request
            if (request == null)
                return BadRequest(new { message = "Request body is required" });

            var createdLog = await _dailyLogService.CreateDailyLogAsync(request);
            return CreatedAtAction(nameof(GetDailyLogById), new { id = createdLog.Id }, createdLog);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error creating daily log");
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Conflict while creating daily log");
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating daily log");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Error creating daily log" });
        }
    }

    /// <summary>
    /// PUT /api/daily
    /// Update an existing daily log.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DailyLogDto>> UpdateDailyLog([FromBody] UpdateDailyLogRequest request)
    {
        try
        {
            _logger.LogInformation($"Updating daily log with ID {request.Id}");

            if (request == null)
                return BadRequest(new { message = "Request body is required" });

            var updatedLog = await _dailyLogService.UpdateDailyLogAsync(request);
            return Ok(updatedLog);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error updating daily log");
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Conflict while updating daily log");
            return Conflict(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating daily log");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Error updating daily log" });
        }
    }

    /// <summary>
    /// DELETE /api/daily/{id}
    /// Delete a daily log.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteDailyLog(int id)
    {
        try
        {
            _logger.LogInformation($"Deleting daily log with ID {id}");

            var result = await _dailyLogService.DeleteDailyLogAsync(id);
            if (!result)
                return NotFound(new { message = $"Daily log with ID {id} not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting daily log with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Error deleting daily log" });
        }
    }
}
