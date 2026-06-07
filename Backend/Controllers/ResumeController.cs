using DailyTracker.API.Models;
using DailyTracker.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DailyTracker.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ResumeController : ControllerBase
{
    private readonly IResumeAnalyzerService _resumeAnalyzerService;
    private readonly ILogger<ResumeController> _logger;

    public ResumeController(IResumeAnalyzerService resumeAnalyzerService, ILogger<ResumeController> logger)
    {
        _resumeAnalyzerService = resumeAnalyzerService;
        _logger = logger;
    }

    [HttpPost("analyze")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ResumeAnalysisResultDto>> AnalyzeResume([FromForm] ResumeAnalysisRequest request)
    {
        if (request == null)
        {
            return BadRequest(new { message = "Request body is required." });
        }

        if (request.ResumeFile == null)
        {
            return BadRequest(new { message = "Resume file is required." });
        }

        if (string.IsNullOrWhiteSpace(request.JobDescription))
        {
            return BadRequest(new { message = "Job description is required." });
        }

        try
        {
            _logger.LogInformation("Analyzing resume: {FileName}", request.ResumeFile.FileName);
            var result = await _resumeAnalyzerService.AnalyzeAsync(request.ResumeFile, request.JobDescription);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            _logger.LogWarning(ex, "Invalid resume analysis request.");
            return BadRequest(new { message = ex.Message });
        }
        catch (NotSupportedException ex)
        {
            _logger.LogWarning(ex, "Unsupported file type for resume analysis.");
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error analyzing resume.");
            return StatusCode(StatusCodes.Status500InternalServerError, new { message = "An error occurred while analyzing the resume.", error = ex.Message });
        }
    }
}
