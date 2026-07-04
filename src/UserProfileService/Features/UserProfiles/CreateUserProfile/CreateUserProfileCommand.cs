using MediatR;

namespace UserProfileService.Features.UserProfiles.CreateUserProfile;

public record CreateUserProfileCommand(
    Guid UserId,
    string FirstName,
    string LastName,
    string Email,
    string Phone
) : IRequest<bool>;