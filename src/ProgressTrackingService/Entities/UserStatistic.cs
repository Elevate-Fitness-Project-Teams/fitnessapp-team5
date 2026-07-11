namespace ProgressTrackingService.Entities;

public class UserStatistic
{
    public int Id { get; private set; }
    public int TotalWorkouts { get; private set; }
    public int TotalCaloriesBurned { get; private set; }
    public double CurrentWeight { get; private set; }
    public double StartWeight { get; private set; }
    public double TotalWeightLost { get; private set; }
    public DateTime UpdatedAt { get; private set; }
}
