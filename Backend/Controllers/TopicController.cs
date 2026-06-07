using DailyTracker.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DailyTracker.API.Controllers;

/// <summary>
/// Controller for managing topics in the full-stack preparation tracker.
/// </summary>
[ApiController]
[Route("api/topics")]
public class TopicController : ControllerBase
{
    private readonly ITopicService _topicService;

    public TopicController(ITopicService topicService)
    {
        _topicService = topicService;
    }

    /// <summary>
    /// Get all topics.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<List<TopicDto>>> GetAllTopics()
    {
        try
        {
            var topics = await _topicService.GetAllTopicsAsync();
            return Ok(topics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching topics", error = ex.Message });
        }
    }

    /// <summary>
    /// Get a topic by ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<TopicDto>> GetTopicById(int id)
    {
        try
        {
            var topic = await _topicService.GetTopicByIdAsync(id);
            if (topic == null)
            {
                return NotFound(new { message = $"Topic with ID {id} not found" });
            }
            return Ok(topic);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching the topic", error = ex.Message });
        }
    }

    /// <summary>
    /// Get topics by category.
    /// </summary>
    [HttpGet("category/{category}")]
    public async Task<ActionResult<List<TopicDto>>> GetTopicsByCategory(string category)
    {
        try
        {
            var topics = await _topicService.GetTopicsByCategoryAsync(category);
            return Ok(topics);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while fetching topics by category", error = ex.Message });
        }
    }

    /// <summary>
    /// Get progress for all categories.
    /// </summary>
    [HttpGet("progress")]
    public async Task<ActionResult<List<TopicProgressDto>>> GetProgress()
    {
        try
        {
            var progress = await _topicService.GetProgressAsync();
            return Ok(progress);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while calculating progress", error = ex.Message });
        }
    }

    /// <summary>
    /// Create a new topic.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TopicDto>> CreateTopic([FromBody] CreateTopicRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var topic = await _topicService.CreateTopicAsync(request);
            return CreatedAtAction(nameof(GetTopicById), new { id = topic.Id }, topic);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the topic", error = ex.Message });
        }
    }

    /// <summary>
    /// Update an existing topic.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<TopicDto>> UpdateTopic(int id, [FromBody] UpdateTopicRequest request)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            request.Id = id;
            var topic = await _topicService.UpdateTopicAsync(request);
            return Ok(topic);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the topic", error = ex.Message });
        }
    }

    /// <summary>
    /// Delete a topic.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTopic(int id)
    {
        try
        {
            var result = await _topicService.DeleteTopicAsync(id);
            if (!result)
            {
                return NotFound(new { message = $"Topic with ID {id} not found" });
            }
            return NoContent();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the topic", error = ex.Message });
        }
    }
}