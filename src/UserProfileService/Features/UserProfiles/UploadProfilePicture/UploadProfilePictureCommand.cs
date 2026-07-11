using MediatR;
using UserProfileService.Abstractions;

namespace UserProfileService.Features.UserProfiles.UploadProfilePicture;

public record UploadProfilePictureCommand(
    IFormFile Image
) : IRequest<Result>;