using MediatR;
using UserProfileService.Abstractions;
using UserProfileService.Entities;
using UserProfileService.Interfaces;
using UserProfileService.Interfaces.Services;

namespace UserProfileService.Features.UserSettings.UpdateSetting;

public class UpdateNotificationCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) 
    : IRequestHandler<UpdateNotificationCommand, Result>
{
    public async Task<Result> Handle(UpdateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = NotificationSetting.CreateStub(currentUser.Id);

        var changedProperties = notification.Update(
            request.WorkoutReminders,
            request.MealReminders,
            request.AchievementAlerts,
            request.EmailNotifications,
            request.WeeklyReports,
            request.PushNotifications);

        if (!changedProperties.Any())
            return Result.Success();

        unitOfWork.Repository<NotificationSetting>().PartialUpdate(notification, changedProperties);

        return Result.Success();
    }
}
