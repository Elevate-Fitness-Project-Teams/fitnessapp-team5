namespace ProgressTrackingService.Entities;

public class WorkoutLog
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int WorkoutId { get; set; }
    public string SessionId { get; set; } = string.Empty;
}
