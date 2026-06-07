namespace DailyTracker.API.Services;

/// <summary>
/// DTO used to create a daily log by date.
/// </summary>
public class CreateDailyLogRequest
{
    public DateTime Date { get; set; }
}

public class UpdateDailyLogRequest
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
}

public class DailyLogDto
{
    public int Id { get; set; }
    public DateTime Date { get; set; }
}

/// <summary>
/// Service interface for daily log business logic.
/// </summary>
public interface IDailyLogService
{
    Task<List<DailyLogDto>> GetAllDailyLogsAsync();
    Task<DailyLogDto?> GetDailyLogByIdAsync(int id);
    Task<DailyLogDto> CreateDailyLogAsync(CreateDailyLogRequest request);
    Task<DailyLogDto> UpdateDailyLogAsync(UpdateDailyLogRequest request);
    Task<bool> DeleteDailyLogAsync(int id);
}
