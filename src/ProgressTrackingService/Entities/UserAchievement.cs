namespace ProgressTrackingService.Entities;

public class UserAchievement
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public int AchievementId { get; set; }
    public DateTime EarnedAt { get; set; }

    public Achievement Achievement { get; set; } = default!;
}
