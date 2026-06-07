using DailyTracker.API.Models;
using DailyTracker.API.Repositories;

namespace DailyTracker.API.Services;

/// <summary>
/// Service implementation for daily log business logic.
/// </summary>
public class DailyLogService : IDailyLogService
{
    private readonly IDailyLogRepository _repository;

    public DailyLogService(IDailyLogRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<DailyLogDto>> GetAllDailyLogsAsync()
    {
        var logs = await _repository.GetAllAsync();
        return logs.Select(MapToDto).ToList();
    }

    public async Task<DailyLogDto?> GetDailyLogByIdAsync(int id)
    {
        var log = await _repository.GetByIdAsync(id);
        return log != null ? MapToDto(log) : null;
    }

    public async Task<DailyLogDto> CreateDailyLogAsync(CreateDailyLogRequest request)
    {
        if (request.Date == default)
            throw new ArgumentException("A valid date is required.");

        var existingLog = await _repository.GetByDateAsync(request.Date);
        if (existingLog != null)
            throw new InvalidOperationException($"A log entry already exists for {request.Date:yyyy-MM-dd}");

        var dailyLog = new DailyLog
        {
            Date = request.Date.Date,
            CreatedAt = DateTime.UtcNow
        };

        var createdLog = await _repository.CreateAsync(dailyLog);
        return MapToDto(createdLog);
    }

    public async Task<DailyLogDto> UpdateDailyLogAsync(UpdateDailyLogRequest request)
    {
        var log = await _repository.GetByIdAsync(request.Id);
        if (log == null)
            throw new ArgumentException($"Daily log with ID {request.Id} not found");

        if (request.Date == default)
            throw new ArgumentException("A valid date is required.");

        if (log.Date.Date != request.Date.Date)
        {
            var conflictingLog = await _repository.GetByDateAsync(request.Date);
            if (conflictingLog != null)
                throw new InvalidOperationException($"A log entry already exists for {request.Date:yyyy-MM-dd}");
        }

        log.Date = request.Date.Date;
        log.UpdatedAt = DateTime.UtcNow;

        var updatedLog = await _repository.UpdateAsync(log);
        return MapToDto(updatedLog);
    }

    public async Task<bool> DeleteDailyLogAsync(int id)
    {
        return await _repository.DeleteAsync(id);
    }

    private static DailyLogDto MapToDto(DailyLog log)
    {
        return new DailyLogDto
        {
            Id = log.Id,
            Date = log.Date
        };
    }
}
