using DailyTracker.API.Data;
using DailyTracker.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DailyTracker.API.Repositories;

/// <summary>
/// Repository implementation for Topic data access operations.
/// </summary>
public class TopicRepository : ITopicRepository
{
    private readonly ApplicationDbContext _context;

    public TopicRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all topics.
    /// </summary>
    public async Task<List<Topic>> GetAllAsync()
    {
        return await _context.Topics
            .AsNoTracking()
            .OrderBy(t => t.Category)
            .ThenBy(t => t.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Get a topic by ID.
    /// </summary>
    public async Task<Topic?> GetByIdAsync(int id)
    {
        return await _context.Topics
            .AsNoTracking()
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    /// <summary>
    /// Get topics by category.
    /// </summary>
    public async Task<List<Topic>> GetByCategoryAsync(string category)
    {
        return await _context.Topics
            .AsNoTracking()
            .Where(t => t.Category == category)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Get completed topics.
    /// </summary>
    public async Task<List<Topic>> GetCompletedAsync()
    {
        return await _context.Topics
            .AsNoTracking()
            .Where(t => t.IsCompleted)
            .OrderBy(t => t.Category)
            .ThenBy(t => t.Name)
            .ToListAsync();
    }

    /// <summary>
    /// Create a new topic.
    /// </summary>
    public async Task<Topic> CreateAsync(Topic topic)
    {
        topic.CreatedAt = DateTime.UtcNow;
        _context.Topics.Add(topic);
        await _context.SaveChangesAsync();
        return topic;
    }

    /// <summary>
    /// Update an existing topic.
    /// </summary>
    public async Task<Topic> UpdateAsync(Topic topic)
    {
        topic.UpdatedAt = DateTime.UtcNow;
        _context.Topics.Update(topic);
        await _context.SaveChangesAsync();
        return topic;
    }

    /// <summary>
    /// Delete a topic.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var topic = await _context.Topics.FindAsync(id);
        if (topic == null)
        {
            return false;
        }

        _context.Topics.Remove(topic);
        await _context.SaveChangesAsync();
        return true;
    }
}