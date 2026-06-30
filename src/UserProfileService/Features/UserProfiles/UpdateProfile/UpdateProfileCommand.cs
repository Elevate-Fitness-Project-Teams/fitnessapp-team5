using MediatR;
using UserProfileService.Abstractions;

namespace UserProfileService.Features.UserProfiles.UpdateProfile;

public record UpdateProfileCommand(
    string FirstName,
    string LastName,
    string Email,   
    string Phone
) : IRequest<Result>;