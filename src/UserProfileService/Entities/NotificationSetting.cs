namespace UserProfileService.Entities;

public sealed class NotificationSetting
{
    public Guid UserId { get; set; }
    public bool WorkoutReminders { get; set; } = true;
    public bool MealReminders { get; set; } = true;
    public bool AchievementAlerts { get; set; } = true;
    public bool EmailNotifications { get; set; } = true;
    public bool WeeklyReports { get; set; } = true;
    public bool PushNotifications { get; set; } = true;

    public UserProfile UserProfile { get; set; } = default!;
}
