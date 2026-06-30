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
        var emailIsExists = await unitOfWork
            .Repository<UserProfile>()
            .GetQueryable()
            .AnyAsync(x => x.Email == request.Email && x.UserId != currentUser.Id, cancellationToken);

        if (emailIsExists)
            return Result.Failure(UserErrors.EmailIsExists);

        var profile = UserProfile.CreateInstance(currentUser.Id);
        profile.Update(request.FirstName, request.LastName, request.Email, request.Phone);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
