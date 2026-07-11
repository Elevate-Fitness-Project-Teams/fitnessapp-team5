using MediatR;
using Microsoft.EntityFrameworkCore;
using UserProfileService.Abstractions;
using UserProfileService.Entities;
using UserProfileService.Errors;
using UserProfileService.Interfaces;
using UserProfileService.Interfaces.Services;

namespace UserProfileService.Features.UserProfiles.UpdateProfile;

internal class UpdateProfileCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser) : IRequestHandler<UpdateProfileCommand, Result>
{
    public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
    {
        var rowsAffected = await unitOfWork
            .Repository<UserProfile>()
            .GetQueryable()
            .Where(x => x.UserId == currentUser.Id)
            .ExecuteUpdateAsync(s => s
            .SetProperty(x => x.FirstName, request.FirstName)
            .SetProperty(x => x.LastName, request.LastName)
            .SetProperty(x => x.Phone, request.Phone)
            , cancellationToken);

        if (rowsAffected == 0)
            return Result.Failure(UserErrors.NotFound);

        return Result.Success();
    }
}
