using MediatR;
using UserProfileService.Entities;
using UserProfileService.Interfaces;

namespace UserProfileService.Features.UserProfiles.CreateUserProfile;

public class CreateUserProfileCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<CreateUserProfileCommand, bool>
{
    public async Task<bool> Handle(CreateUserProfileCommand request, CancellationToken cancellationToken)
    {
        var userProfile = UserProfile.
            Create(request.UserId, request.FirstName, request.LastName, request.Email, request.Phone);

        var preferences = UserPreference.CreateStub(request.UserId);
        var notifications = NotificationSetting.CreateStub(request.UserId);
        var privacy = PrivacySetting.CreateStub(request.UserId);

        await unitOfWork.Repository<UserProfile>().AddAsync(userProfile, cancellationToken);
        await unitOfWork.Repository<UserPreference>().AddAsync(preferences, cancellationToken);
        await unitOfWork.Repository<NotificationSetting>().AddAsync(notifications, cancellationToken);
        await unitOfWork.Repository<PrivacySetting>().AddAsync(privacy, cancellationToken);

        var rowsAffected = await unitOfWork.SaveChangesAsync(cancellationToken);

        return rowsAffected > 0;
    }
}
