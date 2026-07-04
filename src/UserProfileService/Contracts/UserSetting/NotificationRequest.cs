namespace UserProfileService.Contracts.UserSetting;

public record NotificationRequest(
    bool WorkoutReminders,
    bool MealReminders,
    bool AchievementAlerts,
    bool EmailNotifications,
    bool WeeklyReports,
    bool PushNotifications
);

public record PreferencesRequest(
    string Language,
    string Theme,
    string WeightUnit,
    string HeightUnit,
    string DistanceUnit
);

public record PrivacyRequest(
    string ProfileVisibility,
    bool ShowProgressToFriends,
    bool AllowDataSharing
);