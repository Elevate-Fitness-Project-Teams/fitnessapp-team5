using MediatR;
using Microsoft.EntityFrameworkCore;
using UserProfileService.Abstractions;
using UserProfileService.Contracts.UserSetting;
using UserProfileService.Entities;
using UserProfileService.Interfaces;
using UserProfileService.Interfaces.Services;

namespace UserProfileService.Features.UserSettings.GetUserSetting;

public record GetUserSettingQuery() : IRequest<Result<UserSettingResponse>>;

public class GetUserSettingQueryHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<GetUserSettingQuery, Result<UserSettingResponse>>
{
    public async Task<Result<UserSettingResponse>> Handle(GetUserSettingQuery request, CancellationToken cancellationToken)
    {
        var userSetting = await unitOfWork
            .Repository<UserProfile>()
            .GetQueryable()
            .Where(x => x.UserId == currentUser.Id)
            .Select(u => new UserSettingResponse(
                u.UserId,
                new PreferencesResponse(
                    u.Preference.Language,
                    u.Preference.Theme,
                    u.Preference.WeightUnit,
                    u.Preference.HeightUnit,
                    u.Preference.DistanceUnit
                    ),
                new NotificationResponse(
                    u.NotificationSetting.WorkoutReminders,
                    u.NotificationSetting.MealReminders,
                    u.NotificationSetting.AchievementAlerts,
                    u.NotificationSetting.EmailNotifications,
                    u.NotificationSetting.WeeklyReports,
                    u.NotificationSetting.PushNotifications
                    ),
                new PrivacyResponse(
                    u.PrivacySetting.ProfileVisibility,
                    u.PrivacySetting.ShowProgressToFriends,
                    u.PrivacySetting.AllowDataSharing
                    )
                ))
            .FirstOrDefaultAsync(cancellationToken);

        return Result.Success(userSetting!);
    }
}