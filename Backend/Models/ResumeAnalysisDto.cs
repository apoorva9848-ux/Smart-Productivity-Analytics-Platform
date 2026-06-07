using Microsoft.AspNetCore.Http;

namespace DailyTracker.API.Models;

/// <summary>
/// Request model for resume analysis.
/// </summary>
public class ResumeAnalysisRequest
{
    /// <summary>
    /// Uploaded resume file.
    /// </summary>
    public IFormFile? ResumeFile { get; set; }

    /// <summary>
    /// Job description used to compare resume alignment.
    /// </summary>
    public string JobDescription { get; set; } = string.Empty;
}

/// <summary>
/// Response model for resume analysis results.
/// </summary>
public class ResumeAnalysisResultDto
{
    public int MatchScore { get; set; }
    public int AtsScore { get; set; }
    public List<string> MissingSkills { get; set; } = new();
    public List<string> StrongMatches { get; set; } = new();
    public List<string> Recommendations { get; set; } = new();
    public string Summary { get; set; } = string.Empty;
}
