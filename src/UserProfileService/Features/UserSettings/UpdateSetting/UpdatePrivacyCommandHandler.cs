using MediatR;
using UserProfileService.Abstractions;
using UserProfileService.Entities;
using UserProfileService.Interfaces;
using UserProfileService.Interfaces.Services;

namespace UserProfileService.Features.UserSettings.UpdateSetting;

public class UpdatePrivacyCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<UpdatePrivacyCommand, Result>
{
    public async Task<Result> Handle(UpdatePrivacyCommand request, CancellationToken cancellationToken)
    {
        var privacy = PrivacySetting.CreateStub(currentUser.Id);

        var changedProperties = privacy.Update(request.ProfileVisibility, request.ShowProgressToFriends, request.AllowDataSharing);

        if (!changedProperties.Any())
            return Result.Success();

        unitOfWork.Repository<PrivacySetting>().PartialUpdate(privacy, changedProperties);

        return Result.Success();
    }
}