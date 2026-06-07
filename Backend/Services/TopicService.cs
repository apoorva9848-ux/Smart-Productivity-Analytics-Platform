using DailyTracker.API.Models;
using DailyTracker.API.Repositories;

namespace DailyTracker.API.Services;

/// <summary>
/// Service implementation for Topic business logic.
/// </summary>
public class TopicService : ITopicService
{
    private readonly ITopicRepository _topicRepository;

    public TopicService(ITopicRepository topicRepository)
    {
        _topicRepository = topicRepository;
    }

    /// <summary>
    /// Get all topics.
    /// </summary>
    public async Task<List<TopicDto>> GetAllTopicsAsync()
    {
        var topics = await _topicRepository.GetAllAsync();
        return topics.Select(MapToDto).ToList();
    }

    /// <summary>
    /// Get a topic by ID.
    /// </summary>
    public async Task<TopicDto?> GetTopicByIdAsync(int id)
    {
        var topic = await _topicRepository.GetByIdAsync(id);
        return topic != null ? MapToDto(topic) : null;
    }

    /// <summary>
    /// Get topics by category.
    /// </summary>
    public async Task<List<TopicDto>> GetTopicsByCategoryAsync(string category)
    {
        var topics = await _topicRepository.GetByCategoryAsync(category);
        return topics.Select(MapToDto).ToList();
    }

    /// <summary>
    /// Get progress for all categories.
    /// </summary>
    public async Task<List<TopicProgressDto>> GetProgressAsync()
    {
        var allTopics = await _topicRepository.GetAllAsync();
        var categories = allTopics.GroupBy(t => t.Category);

        var progress = new List<TopicProgressDto>();
        foreach (var category in categories)
        {
            var total = category.Count();
            var completed = category.Count(t => t.IsCompleted);
            var percentage = total > 0 ? (double)completed / total * 100 : 0;

            progress.Add(new TopicProgressDto
            {
                Category = category.Key,
                Completed = completed,
                Total = total,
                Percentage = Math.Round(percentage, 1)
            });
        }

        return progress.OrderBy(p => p.Category).ToList();
    }

    /// <summary>
    /// Create a new topic.
    /// </summary>
    public async Task<TopicDto> CreateTopicAsync(CreateTopicRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Topic name cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(request.Category))
        {
            throw new ArgumentException("Topic category cannot be empty");
        }

        var topic = new Topic
        {
            Name = request.Name.Trim(),
            Category = request.Category.Trim(),
            Notes = request.Notes?.Trim()
        };

        var createdTopic = await _topicRepository.CreateAsync(topic);
        return MapToDto(createdTopic);
    }

    /// <summary>
    /// Update an existing topic.
    /// </summary>
    public async Task<TopicDto> UpdateTopicAsync(UpdateTopicRequest request)
    {
        var existingTopic = await _topicRepository.GetByIdAsync(request.Id);
        if (existingTopic == null)
        {
            throw new KeyNotFoundException($"Topic with ID {request.Id} not found");
        }

        if (string.IsNullOrWhiteSpace(request.Name))
        {
            throw new ArgumentException("Topic name cannot be empty");
        }

        if (string.IsNullOrWhiteSpace(request.Category))
        {
            throw new ArgumentException("Topic category cannot be empty");
        }

        existingTopic.Name = request.Name.Trim();
        existingTopic.Category = request.Category.Trim();
        existingTopic.IsCompleted = request.IsCompleted;
        existingTopic.DateCompleted = request.IsCompleted ? (request.DateCompleted ?? DateTime.UtcNow.Date) : null;
        existingTopic.Notes = request.Notes?.Trim();

        var updatedTopic = await _topicRepository.UpdateAsync(existingTopic);
        return MapToDto(updatedTopic);
    }

    /// <summary>
    /// Delete a topic.
    /// </summary>
    public async Task<bool> DeleteTopicAsync(int id)
    {
        return await _topicRepository.DeleteAsync(id);
    }

    private static TopicDto MapToDto(Topic topic)
    {
        return new TopicDto
        {
            Id = topic.Id,
            Name = topic.Name,
            Category = topic.Category,
            IsCompleted = topic.IsCompleted,
            DateCompleted = topic.DateCompleted,
            Notes = topic.Notes
        };
    }
}