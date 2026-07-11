using MediatR;
using Microsoft.EntityFrameworkCore;
using UserProfileService.Abstractions;
using UserProfileService.Entities;
using UserProfileService.Errors;
using UserProfileService.Interfaces;
using UserProfileService.Interfaces.Services;

namespace UserProfileService.Features.UserProfiles.UploadProfilePicture;

public class UploadProfilePictureCommandHandler(IUnitOfWork unitOfWork,
    IFileService fileService, ICurrentUser currentUser) : IRequestHandler<UploadProfilePictureCommand, Result>
{
    public async Task<Result> Handle(UploadProfilePictureCommand request, CancellationToken cancellationToken)
    {
        var repo = unitOfWork.Repository<UserProfile>().GetQueryable().Where(x => x.UserId == currentUser.Id);

        var oldImage = await repo
            .Select(x => x.ProfilePictureUrl)
            .FirstOrDefaultAsync(cancellationToken);

        if (oldImage is not null)
            await fileService.DeleteAsync(oldImage);

        var pictureUrl = await fileService.UploadAsync(request.Image, cancellationToken);

        var rowsAffected = await repo
            .ExecuteUpdateAsync(s => s
            .SetProperty(x => x.ProfilePictureUrl, pictureUrl)
            , cancellationToken);

        if (rowsAffected == 0)
            return Result.Failure(UserErrors.UserPicture);

        return Result.Success();
    }
}
