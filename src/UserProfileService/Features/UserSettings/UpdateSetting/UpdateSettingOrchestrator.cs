using Mapster;
using MediatR;
using UserProfileService.Abstractions;
using UserProfileService.Contracts.UserSetting;
using UserProfileService.Errors;
using UserProfileService.Interfaces;

namespace UserProfileService.Features.UserSettings.UpdateSetting;

public record UpdateSettingOrchestrator(
    NotificationRequest notificationRequest,
    PrivacyRequest privacyRequest,
    PreferencesRequest preferencesRequest
) : IRequest<Result>;

public class UpdateSettingOrchestratorHandler(ISender sender, IUnitOfWork unitOfWork) : IRequestHandler<UpdateSettingOrchestrator, Result>
{
    public async Task<Result> Handle(UpdateSettingOrchestrator request, CancellationToken cancellationToken)
    {
        var notificationResult = await sender.Send(request.notificationRequest.Adapt<UpdateNotificationCommand>(), cancellationToken);

        var privacyResult = await sender.Send(request.privacyRequest.Adapt<UpdatePrivacyCommand>(), cancellationToken);

        var preferencesResult = await sender.Send(request.preferencesRequest.Adapt<UpdatePreferencesCommand>(), cancellationToken);

        var rowsAffected = await unitOfWork.SaveChangesAsync(cancellationToken);

        return rowsAffected > 0 ? Result.Success() : Result.Failure(UserErrors.UpdateSettingFailed);
    }
}