using MediatR;
using UserProfileService.Abstractions;

namespace UserProfileService.Features.UserSettings.UpdateSetting;

public record UpdateNotificationCommand(
    bool WorkoutReminders,
    bool MealReminders,
    bool AchievementAlerts,
    bool EmailNotifications,
    bool WeeklyReports,
    bool PushNotifications
) : IRequest<Result>;
