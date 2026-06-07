using DailyTracker.API.Models;
using DailyTracker.API.Repositories;
using Microsoft.AspNetCore.Http;
using System.IO.Compression;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using UglyToad.PdfPig;

namespace DailyTracker.API.Services;

public class ResumeAnalyzerService : IResumeAnalyzerService
{
    private readonly ITopicRepository _topicRepository;
    private readonly ILogger<ResumeAnalyzerService> _logger;

    // Technical keyword dictionary (canonical skill phrases).
    private static readonly string[] TechnicalKeywords = new[]
    {
        "Angular",
        "TypeScript",
        "JavaScript",
        "HTML",
        "CSS",
        ".NET",
        "ASP.NET Core",
        "C#",
        "SQL",
        "Azure",
        "Git",
        "REST API",
        "Entity Framework",
        "EF Core",
        "JWT",
        "LINQ",
        "Docker",
        "Kubernetes",
        "Microservices",
        "React",
        "Node.js",
        "AWS",
        "GCP",
        "CI/CD",
        "Azure DevOps",
        "SQL Server",
        "Identity",
        "OAuth",
        "NoSQL",
        "Redis",
        "GraphQL",
        "Webpack",
        "Babel",
        "Sass",
        "Less"
    };

    // Stop words to ignore when extracting skills from free text
    private static readonly HashSet<string> StopWords = new(StringComparer.OrdinalIgnoreCase)
    {
        "job", "description", "title", "role", "candidate", "location", "required", "preferred",
        "years", "experience", "and", "or", "the", "a", "an", "with", "for", "only"
    };

    private static readonly Dictionary<string, string[]> SkillSynonyms = new(StringComparer.OrdinalIgnoreCase)
    {
        { ".NET", new[] { "dotnet", "dot net", "csharp", "c sharp", "c#", "asp.net core", "aspnet core", "asp net core" } },
        { "C#", new[] { "csharp", "c sharp", "dotnet", "dot net", ".net", "asp.net core", "aspnet core", "asp net core" } },
        { "ASP.NET Core", new[] { "aspnet core", "asp.net core", "asp net core", ".net core", ".net", "dotnet", "dot net", "csharp", "c sharp", "c#" } },
        { "REST API", new[] { "rest api", "restapi" } },
        { "EF Core", new[] { "ef core", "efcore" } },
        { "SQL Server", new[] { "sql server", "sqlserver" } },
        { "CI/CD", new[] { "ci/cd", "cicd" } },
    };

    // Weight map for important skills. Higher weight gives more influence on ATS/readiness.
    private static readonly HashSet<string> HighWeightSkills = new(StringComparer.OrdinalIgnoreCase)
    {
        "Angular","TypeScript","JavaScript","C#",".NET","ASP.NET Core","SQL","Azure",
        "Docker","Kubernetes","Microservices","Entity Framework","EF Core","REST API","Git"
    };

    public ResumeAnalyzerService(ITopicRepository topicRepository, ILogger<ResumeAnalyzerService> logger)
    {
        _topicRepository = topicRepository;
        _logger = logger;
    }

    public async Task<ResumeAnalysisResultDto> AnalyzeAsync(IFormFile resumeFile, string jobDescription)
    {
        if (resumeFile == null || resumeFile.Length == 0)
        {
            throw new ArgumentException("Resume file is required.");
        }

        if (string.IsNullOrWhiteSpace(jobDescription))
        {
            throw new ArgumentException("Job description is required.");
        }

        var resumeText = await ExtractTextAsync(resumeFile);
        if (string.IsNullOrWhiteSpace(resumeText))
        {
            throw new InvalidOperationException("Unable to extract text from the uploaded resume.");
        }

        var resumeNormalized = NormalizeText(resumeText);
        var jobNormalized = NormalizeText(jobDescription);

        // Extract only technical skills from both resume and job description using the dictionary
        var jobTechnicalSkills = ExtractTechnicalSkills(jobDescription);
        var resumeTechnicalSkills = ExtractTechnicalSkills(resumeText);

        // Matched and missing skills are strictly technical
        var strongMatches = jobTechnicalSkills
            .Where(jobSkill => resumeTechnicalSkills.Any(resumeSkill => SkillsAreEquivalent(jobSkill, resumeSkill)))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        var missingSkills = jobTechnicalSkills
            .Except(strongMatches, StringComparer.OrdinalIgnoreCase)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        // Calculate weighted match percentage
        var weightedMatchPct = CalculateWeightedMatchPercentage(jobTechnicalSkills, strongMatches);
        var matchScore = (int)Math.Round(weightedMatchPct);

        var atsScore = CalculateAtsScoreWeighted(resumeNormalized, weightedMatchPct, strongMatches.Count);

        var recommendations = BuildRecommendations(resumeNormalized, strongMatches, missingSkills, atsScore);
        await ApplyTopicRecommendations(resumeNormalized, missingSkills, recommendations);

        return new ResumeAnalysisResultDto
        {
            MatchScore = matchScore,
            AtsScore = atsScore,
            MissingSkills = missingSkills.Take(20).ToList(),
            StrongMatches = strongMatches.Take(20).ToList(),
            Recommendations = recommendations,
            Summary = BuildSummary(matchScore, atsScore, missingSkills)
        };
    }

    private async Task ApplyTopicRecommendations(string resumeNormalized, List<string> missingSkills, List<string> recommendations)
    {
        var topics = await _topicRepository.GetAllAsync();
        if (!topics.Any())
        {
            return;
        }

        var trackedTerms = topics
            .SelectMany(t => new[] { t.Name, t.Category })
            .Where(x => !string.IsNullOrWhiteSpace(x))
            .Select(x => NormalizeText(x))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        foreach (var missingSkill in missingSkills)
        {
            if (trackedTerms.Any(term => term.Contains(missingSkill, StringComparison.OrdinalIgnoreCase) || missingSkill.Contains(term, StringComparison.OrdinalIgnoreCase)))
            {
                recommendations.Add($"You track {missingSkill} in your learning roadmap. Complete it and mention it on your resume to close the gap.");
                break;
            }
        }

        if (!trackedTerms.Any(term => missingSkills.Any(skill => term.Contains(skill, StringComparison.OrdinalIgnoreCase) || skill.Contains(term, StringComparison.OrdinalIgnoreCase))))
        {
            var requested = missingSkills.Take(3).ToList();
            if (requested.Any())
            {
                recommendations.Add($"Consider adding {string.Join(", ", requested)} to your learning roadmap so the resume and tracker stay aligned.");
            }
        }
    }

    private string BuildSummary(int matchScore, int atsScore, List<string> missingSkills)
    {
        if (matchScore >= 80 && atsScore >= 75)
        {
            return "Your resume is well aligned with the target role, with solid keyword coverage and good ATS structure.";
        }

        if (matchScore < 40)
        {
            return "Your resume needs stronger job description alignment. Focus on adding key skills and concrete experience details.";
        }

        return missingSkills.Count == 0
            ? "Your resume is a strong match. Keep the structure concise and the skills section visible."
            : "The resume is a reasonable baseline, but it needs more job-specific keyword coverage and ATS-friendly structure.";
    }

    private int CalculateAtsScore(string resumeNormalized, int matchScore, int strongMatches)
    {
        var sectionScore = 0;

        if (resumeNormalized.Contains("technical skills"))
        {
            sectionScore += 20;
        }

        if (resumeNormalized.Contains("experience") || resumeNormalized.Contains("work experience"))
        {
            sectionScore += 20;
        }

        if (resumeNormalized.Contains("project") || resumeNormalized.Contains("projects"))
        {
            sectionScore += 20;
        }

        if (resumeNormalized.Contains("education"))
        {
            sectionScore += 20;
        }

        if (strongMatches >= 5)
        {
            sectionScore += 20;
        }

        return Math.Clamp((int)Math.Round(matchScore * 0.65 + sectionScore * 0.35), 0, 100);
    }

    private List<string> BuildRecommendations(string resumeNormalized, List<string> strongMatches, List<string> missingSkills, int atsScore)
    {
        var recommendations = new List<string>();

        if (!resumeNormalized.Contains("skills"))
        {
            recommendations.Add("Add a clear technical skills section near the top of your resume.");
        }

        if (!resumeNormalized.Contains("experience") && !resumeNormalized.Contains("work experience"))
        {
            recommendations.Add("Add a concise experience section with project titles, technologies used, and measurable outcomes.");
        }

        if (missingSkills.Any())
        {
            recommendations.Add($"Add keywords from the job description such as {string.Join(", ", missingSkills.Take(5))}.");
        }
        else
        {
            recommendations.Add("Your resume already includes the main job keywords. Keep the skills and experience sections strong.");
        }

        if (atsScore < 70)
        {
            recommendations.Add("Make sure your resume uses standard headings like Skills, Experience, Projects, and Education for better ATS parsing.");
        }

        if (!resumeNormalized.Contains("achievement") && !resumeNormalized.Contains("improved"))
        {
            recommendations.Add("Add measurable achievements and concrete impact statements rather than generic responsibilities.");
        }

        if (strongMatches.Count > 0)
        {
            recommendations.Add($"Keep the strength of your matched skills visible: {string.Join(", ", strongMatches.Take(4))}.");
        }

        return recommendations.Distinct().ToList();
    }

    private static List<string> ExtractKeywords(string text)
    {
        var normalized = NormalizeText(text);
        var words = Regex.Matches(normalized, @"[\p{L}\d\+#]+(?:\/[\p{L}\d\+#]+)?")
            .Select(match => match.Value.Trim().ToLowerInvariant())
            .Where(token => token.Length > 1 && !StopWords.Contains(token))
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToList();

        return words;
    }

    private static bool ContainsKeyword(string text, string term)
    {
        if (string.IsNullOrWhiteSpace(text) || string.IsNullOrWhiteSpace(term))
        {
            return false;
        }

        return Regex.IsMatch(text, "(^|\\W)" + Regex.Escape(term) + "($|\\W)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
    }

    private static bool SkillsAreEquivalent(string skillA, string skillB)
    {
        if (string.Equals(skillA, skillB, StringComparison.OrdinalIgnoreCase))
        {
            return true;
        }

        var equivalenceGroups = new[]
        {
            new[] { ".NET", "C#", "ASP.NET Core" },
        };

        return equivalenceGroups.Any(group =>
            group.Contains(skillA, StringComparer.OrdinalIgnoreCase)
            && group.Contains(skillB, StringComparer.OrdinalIgnoreCase));
    }

    private static List<string> ExtractTechnicalSkills(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return new List<string>();
        var normalized = NormalizeText(text);
        var normalizedSearch = NormalizeForSearch(text);

        var sortedKeywords = TechnicalKeywords.OrderByDescending(k => k.Length).ToList();
        var found = new List<string>();

        foreach (var kw in sortedKeywords)
        {
            var keywordLower = NormalizeText(kw);
            var keywordSearch = NormalizeForSearch(kw);

            if (ContainsKeyword(normalized, keywordLower)
                || ContainsKeyword(normalizedSearch, keywordSearch))
            {
                found.Add(kw);
                continue;
            }

            if (SkillSynonyms.TryGetValue(kw, out var variants))
            {
                foreach (var variant in variants)
                {
                    var normalizedVariant = NormalizeText(variant);
                    var normalizedSearchVariant = NormalizeForSearch(variant);

                    if (ContainsKeyword(normalized, normalizedVariant)
                        || ContainsKeyword(normalizedSearch, normalizedSearchVariant))
                    {
                        found.Add(kw);
                        break;
                    }
                }
            }
        }

        return found.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
    }

    private static double CalculateWeightedMatchPercentage(List<string> jobSkills, List<string> matchedSkills)
    {
        if (jobSkills == null || !jobSkills.Any()) return 0.0;

        double totalWeight = 0.0;
        double matchedWeight = 0.0;

        foreach (var s in jobSkills)
        {
            var weight = HighWeightSkills.Contains(s, StringComparer.OrdinalIgnoreCase) ? 2.0 : 1.0;
            totalWeight += weight;
            if (matchedSkills.Any(m => SkillsAreEquivalent(m, s)))
            {
                matchedWeight += weight;
            }
        }

        if (totalWeight <= 0) return 0.0;
        return (matchedWeight / totalWeight) * 100.0;
    }

    private int CalculateAtsScoreWeighted(string resumeNormalized, double weightedMatchPct, int strongMatchesCount)
    {
        var sectionScore = 0;

        if (resumeNormalized.Contains("technical skills"))
        {
            sectionScore += 25;
        }

        if (resumeNormalized.Contains("experience") || resumeNormalized.Contains("work experience"))
        {
            sectionScore += 20;
        }

        if (resumeNormalized.Contains("project") || resumeNormalized.Contains("projects"))
        {
            sectionScore += 15;
        }

        if (resumeNormalized.Contains("education"))
        {
            sectionScore += 10;
        }

        if (strongMatchesCount >= 5)
        {
            sectionScore += 15;
        }

        // Combine weighted match percentage with structural section score
        var ats = Math.Clamp((int)Math.Round(weightedMatchPct * 0.7 + sectionScore * 0.3), 0, 100);
        return ats;
    }

    private static string NormalizeText(string text)
    {
        return Regex.Replace(text, "[\r\n\t]+", " ", RegexOptions.Compiled).Trim().ToLowerInvariant();
    }

    private static string NormalizeForSearch(string text)
    {
        var normalized = NormalizeText(text);
        return Regex.Replace(normalized, "[^a-z0-9]+", " ", RegexOptions.Compiled).Trim();
    }

    private static async Task<string> ExtractTextAsync(IFormFile resumeFile)
    {
        var extension = Path.GetExtension(resumeFile.FileName).ToLowerInvariant();
        return extension switch
        {
            ".pdf" => await ExtractTextFromPdfAsync(resumeFile),
            ".docx" => await ExtractTextFromDocxAsync(resumeFile),
            ".txt" => await ExtractTextFromTextAsync(resumeFile),
            _ => throw new NotSupportedException("Only PDF, DOCX, and TXT files are supported.")
        };
    }

    private static async Task<string> ExtractTextFromTextAsync(IFormFile resumeFile)
    {
        using var stream = resumeFile.OpenReadStream();
        using var reader = new StreamReader(stream, Encoding.UTF8, true);
        return await reader.ReadToEndAsync();
    }

    private static async Task<string> ExtractTextFromDocxAsync(IFormFile resumeFile)
    {
        using var memoryStream = new MemoryStream();
        await resumeFile.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        using var archive = new ZipArchive(memoryStream, ZipArchiveMode.Read, true);
        var entry = archive.GetEntry("word/document.xml");
        if (entry == null)
        {
            return string.Empty;
        }

        using var entryStream = entry.Open();
        var xmlDoc = new XmlDocument();
        xmlDoc.Load(entryStream);

        var builder = new StringBuilder();
        var nodes = xmlDoc.GetElementsByTagName("t");
        foreach (XmlNode node in nodes)
        {
            if (!string.IsNullOrWhiteSpace(node.InnerText))
            {
                builder.Append(node.InnerText).Append(' ');
            }
        }

        return builder.ToString();
    }

    private static async Task<string> ExtractTextFromPdfAsync(IFormFile resumeFile)
    {
        using var memoryStream = new MemoryStream();
        await resumeFile.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        try
        {
            using var pdf = PdfDocument.Open(memoryStream);
            var pageBuilder = new StringBuilder();
            foreach (var page in pdf.GetPages())
            {
                var pageText = page.Text;
                if (!string.IsNullOrWhiteSpace(pageText))
                {
                    pageBuilder.Append(pageText).Append(' ');
                }
            }

            if (pageBuilder.Length > 0)
            {
                return pageBuilder.ToString();
            }
        }
        catch
        {
            // Fallback to legacy extraction if PDF parsing fails.
        }

        memoryStream.Seek(0, SeekOrigin.Begin);
        var bytes = memoryStream.ToArray();
        var raw = Encoding.ASCII.GetString(bytes);
        var builder = new StringBuilder();

        var matches = Regex.Matches(raw, @"\(([^\)\n]{3,})\)");
        foreach (Match match in matches)
        {
            builder.Append(match.Groups[1].Value).Append(' ');
        }

        if (builder.Length == 0)
        {
            var text = Regex.Replace(raw, "[^\u0020-\u007E]+", " ");
            builder.Append(text);
        }

        return builder.ToString();
    }
}
