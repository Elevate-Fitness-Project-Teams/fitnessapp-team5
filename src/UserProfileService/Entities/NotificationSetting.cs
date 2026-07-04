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

    private NotificationSetting() { }

    public static NotificationSetting CreateStub(Guid userId) => new() { UserId = userId };

    public IReadOnlyCollection<string> Update(
        bool? workoutReminders, bool? mealReminders, bool? achievementAlerts, bool? emailNotifications, bool? weeklyReports, bool? pushNotifications)
    {
        var changedProperties = new List<string>();

        if (workoutReminders.HasValue && WorkoutReminders != workoutReminders.Value)
        {
            WorkoutReminders = workoutReminders.Value;
            changedProperties.Add(nameof(WorkoutReminders));
        }

        if (mealReminders.HasValue && MealReminders != mealReminders.Value)
        {
            MealReminders = mealReminders.Value;
            changedProperties.Add(nameof(MealReminders));
        }

        if (achievementAlerts.HasValue && AchievementAlerts != achievementAlerts.Value)
        {
            AchievementAlerts = achievementAlerts.Value;
            changedProperties.Add(nameof(AchievementAlerts));
        }

        if (emailNotifications.HasValue && EmailNotifications != emailNotifications.Value)
        {
            EmailNotifications = emailNotifications.Value;
            changedProperties.Add(nameof(EmailNotifications));
        }

        if (weeklyReports.HasValue && WeeklyReports != weeklyReports.Value)
        {
            WeeklyReports = weeklyReports.Value;
            changedProperties.Add(nameof(WeeklyReports));
        }

        if (pushNotifications.HasValue && PushNotifications != pushNotifications.Value)
        {
            PushNotifications = pushNotifications.Value;
            changedProperties.Add(nameof(PushNotifications));
        }

        return changedProperties;
    }
}
