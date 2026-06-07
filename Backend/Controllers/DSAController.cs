using DailyTracker.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DailyTracker.API.Controllers;

/// <summary>
/// API controller for DSA problem tracking operations.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class DSAController : ControllerBase
{
    private readonly IDSAService _dsaService;
    private readonly ILogger<DSAController> _logger;

    public DSAController(IDSAService dsaService, ILogger<DSAController> logger)
    {
        _dsaService = dsaService;
        _logger = logger;
    }

    /// <summary>
    /// GET /api/dsa
    /// Get all DSA problems.
    /// </summary>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<ActionResult<List<DSAProblemDto>>> GetAllProblems()
    {
        try
        {
            _logger.LogInformation("Fetching all DSA problems");
            var problems = await _dsaService.GetAllProblemsAsync();
            return Ok(problems);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching DSA problems");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Error fetching DSA problems" });
        }
    }

    /// <summary>
    /// GET /api/dsa/{id}
    /// Get a specific DSA problem by ID.
    /// </summary>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DSAProblemDto>> GetProblemById(int id)
    {
        try
        {
            _logger.LogInformation($"Fetching DSA problem with ID {id}");
            var problem = await _dsaService.GetProblemByIdAsync(id);

            if (problem == null)
                return NotFound(new { message = $"DSA problem with ID {id} not found" });

            return Ok(problem);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error fetching DSA problem with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Error fetching DSA problem" });
        }
    }

    /// <summary>
    /// GET /api/dsa/filter?difficulty=Medium&pattern=Array
    /// Filter DSA problems by difficulty and/or pattern.
    /// </summary>
    [HttpGet("filter")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<List<DSAProblemDto>>> FilterProblems(
        [FromQuery] string? difficulty,
        [FromQuery] string? pattern,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate)
    {
        try
        {
            _logger.LogInformation($"Filtering DSA problems: difficulty={difficulty}, pattern={pattern}");

            List<DSAProblemDto> problems = new();

            if (!string.IsNullOrWhiteSpace(difficulty))
            {
                problems = await _dsaService.GetProblemsByDifficultyAsync(difficulty);
            }
            else if (!string.IsNullOrWhiteSpace(pattern))
            {
                problems = await _dsaService.GetProblemsByPatternAsync(pattern);
            }
            else if (startDate.HasValue && endDate.HasValue)
            {
                problems = await _dsaService.GetProblemsByDateRangeAsync(startDate.Value, endDate.Value);
            }
            else
            {
                problems = await _dsaService.GetAllProblemsAsync();
            }

            return Ok(problems);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid filter parameters");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error filtering DSA problems");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Error filtering DSA problems" });
        }
    }

    /// <summary>
    /// POST /api/dsa
    /// Create a new DSA problem entry.
    /// </summary>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<DSAProblemDto>> CreateProblem([FromBody] CreateDSAProblemRequest request)
    {
        try
        {
            _logger.LogInformation($"Creating DSA problem: {request.Name}");

            if (request == null)
                return BadRequest(new { message = "Request body is required" });

            var createdProblem = await _dsaService.CreateProblemAsync(request);
            return CreatedAtAction(nameof(GetProblemById), new { id = createdProblem.Id }, createdProblem);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error creating DSA problem");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating DSA problem");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Error creating DSA problem" });
        }
    }

    /// <summary>
    /// PUT /api/dsa
    /// Update an existing DSA problem.
    /// </summary>
    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<DSAProblemDto>> UpdateProblem([FromBody] UpdateDSAProblemRequest request)
    {
        try
        {
            _logger.LogInformation($"Updating DSA problem with ID {request.Id}");

            if (request == null)
                return BadRequest(new { message = "Request body is required" });

            var updatedProblem = await _dsaService.UpdateProblemAsync(request);
            return Ok(updatedProblem);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Validation error updating DSA problem");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating DSA problem");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Error updating DSA problem" });
        }
    }

    /// <summary>
    /// DELETE /api/dsa/{id}
    /// Delete a DSA problem.
    /// </summary>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteProblem(int id)
    {
        try
        {
            _logger.LogInformation($"Deleting DSA problem with ID {id}");

            var result = await _dsaService.DeleteProblemAsync(id);
            if (!result)
                return NotFound(new { message = $"DSA problem with ID {id} not found" });

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error deleting DSA problem with ID {id}");
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { message = "Error deleting DSA problem" });
        }
    }
}
