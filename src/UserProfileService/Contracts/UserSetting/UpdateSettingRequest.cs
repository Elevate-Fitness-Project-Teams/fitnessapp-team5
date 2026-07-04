namespace UserProfileService.Contracts.UserSetting;

public record UpdateSettingRequest(
    NotificationRequest Notification,
    PreferencesRequest Preferences,
    PrivacyRequest Privacy
);