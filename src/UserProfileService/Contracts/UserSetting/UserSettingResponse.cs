namespace UserProfileService.Contracts.UserSetting;

public record UserSettingResponse(
    Guid UserId,
    PreferencesResponse Preferences,
    NotificationResponse Notification,
    PrivacyResponse Privacy
);

public record PreferencesResponse(
    string Language,
    string Theme,
    string WeightUnit,
    string HeightUnit,
    string DistanceUnit
);
public record NotificationResponse(
    bool WorkoutReminders,
    bool MealReminders,
    bool AchievementAlerts,
    bool EmailNotifications,
    bool WeeklyReports,
    bool PushNotifications
);

public record PrivacyResponse(
    string ProfileVisibility,
    bool ShowProgressToFriends,
    bool AllowDataSharing
);