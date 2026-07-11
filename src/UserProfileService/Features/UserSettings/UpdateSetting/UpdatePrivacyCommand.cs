using MediatR;
using UserProfileService.Abstractions;

namespace UserProfileService.Features.UserSettings.UpdateSetting;

public record UpdatePrivacyCommand(
    string ProfileVisibility,
    bool ShowProgressToFriends,
    bool AllowDataSharing
) : IRequest<Result>;