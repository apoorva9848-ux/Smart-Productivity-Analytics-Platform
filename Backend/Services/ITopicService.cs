using DailyTracker.API.Models;

namespace DailyTracker.API.Services;

/// <summary>
/// DTOs for Topic API requests and responses.
/// </summary>
public class CreateTopicRequest
{
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string? Notes { get; set; }
}

public class UpdateTopicRequest
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime? DateCompleted { get; set; }
    public string? Notes { get; set; }
}

public class TopicDto
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public DateTime? DateCompleted { get; set; }
    public string? Notes { get; set; }
}

public class TopicProgressDto
{
    public string Category { get; set; } = string.Empty;
    public int Completed { get; set; }
    public int Total { get; set; }
    public double Percentage { get; set; }
}

/// <summary>
/// Service interface for Topic business logic.
/// </summary>
public interface ITopicService
{
    /// <summary>
    /// Get all topics.
    /// </summary>
    Task<List<TopicDto>> GetAllTopicsAsync();

    /// <summary>
    /// Get a topic by ID.
    /// </summary>
    Task<TopicDto?> GetTopicByIdAsync(int id);

    /// <summary>
    /// Get topics by category.
    /// </summary>
    Task<List<TopicDto>> GetTopicsByCategoryAsync(string category);

    /// <summary>
    /// Get progress for all categories.
    /// </summary>
    Task<List<TopicProgressDto>> GetProgressAsync();

    /// <summary>
    /// Create a new topic.
    /// </summary>
    Task<TopicDto> CreateTopicAsync(CreateTopicRequest request);

    /// <summary>
    /// Update an existing topic.
    /// </summary>
    Task<TopicDto> UpdateTopicAsync(UpdateTopicRequest request);

    /// <summary>
    /// Delete a topic.
    /// </summary>
    Task<bool> DeleteTopicAsync(int id);
}