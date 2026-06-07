using DailyTracker.API.Models;

namespace DailyTracker.API.Repositories;

/// <summary>
/// Repository interface for Topic data access operations.
/// </summary>
public interface ITopicRepository
{
    /// <summary>
    /// Get all topics.
    /// </summary>
    Task<List<Topic>> GetAllAsync();

    /// <summary>
    /// Get a topic by ID.
    /// </summary>
    Task<Topic?> GetByIdAsync(int id);

    /// <summary>
    /// Get topics by category.
    /// </summary>
    Task<List<Topic>> GetByCategoryAsync(string category);

    /// <summary>
    /// Get completed topics.
    /// </summary>
    Task<List<Topic>> GetCompletedAsync();

    /// <summary>
    /// Create a new topic.
    /// </summary>
    Task<Topic> CreateAsync(Topic topic);

    /// <summary>
    /// Update an existing topic.
    /// </summary>
    Task<Topic> UpdateAsync(Topic topic);

    /// <summary>
    /// Delete a topic.
    /// </summary>
    Task<bool> DeleteAsync(int id);
}