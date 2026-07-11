namespace ProgressTrackingService.Entities;

public class WorkoutLog
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int WorkoutId { get; set; }
    public string SessionId { get; set; } = null!;
    public int DurationMinutes { get; set; }
    public int CaloriesBurned { get; set; }
    public int Rating { get; set; }
    public string Notes { get; set; } = null!;
    public DateTime CompletedAt { get; set; }
}
