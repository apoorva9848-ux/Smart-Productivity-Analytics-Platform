using DailyTracker.API.Models;
using Microsoft.AspNetCore.Http;

namespace DailyTracker.API.Services;

public interface IResumeAnalyzerService
{
    Task<ResumeAnalysisResultDto> AnalyzeAsync(IFormFile resumeFile, string jobDescription);
}
