using MediatR;
using UserProfileService.Abstractions;
using UserProfileService.Entities;
using UserProfileService.Interfaces;
using UserProfileService.Interfaces.Services;

namespace UserProfileService.Features.UserSettings.UpdateSetting;

public class UpdatePreferencesCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<UpdatePreferencesCommand, Result>
{
    public async Task<Result> Handle(UpdatePreferencesCommand request, CancellationToken cancellationToken)
    {
        var preferences = UserPreference.CreateStub(currentUser.Id);

        var changedProperties = preferences.Update(request.Language, request.Theme, request.WeightUnit, request.HeightUnit, request.DistanceUnit);

        if (!changedProperties.Any())
            return Result.Success();

        unitOfWork.Repository<UserPreference>().PartialUpdate(preferences, changedProperties);

        return Result.Success();
    }
}
