namespace ProgressTrackingService.Entities;

public class Streak
{
    public Guid UserId { get; set; }
    public int CurrentStreak { get; set; }
    public int LongestStreak { get; set; }
    public DateTime? LastWorkoutDate { get; set; }
}
