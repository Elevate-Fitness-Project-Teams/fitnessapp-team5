using Mapster;
using MediatR;
using Microsoft.EntityFrameworkCore;
using UserProfileService.Abstractions;
using UserProfileService.Contracts.UserProfiles;
using UserProfileService.Entities;
using UserProfileService.Errors;
using UserProfileService.Interfaces;
using UserProfileService.Interfaces.Services;

namespace UserProfileService.Features.UserProfiles.GetUserProfile;

public record GetUserProfileQuery : IRequest<Result<UserProfileResponse>>;

internal class GetUserProfileQueryHandler(IUnitOfWork _unitOfWork, ICurrentUser currentUser) : IRequestHandler<GetUserProfileQuery, Result<UserProfileResponse>>
{
    public async Task<Result<UserProfileResponse>> Handle(GetUserProfileQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork
            .Repository<UserProfile>()
            .GetQueryable()
            .Where(x => x.UserId == currentUser.Id)
            .ProjectToType<UserProfileResponse>()
            .FirstOrDefaultAsync(cancellationToken);

        if (user is null)
            return Result.Failure<UserProfileResponse>(UserErrors.InvalidToken);

        return Result.Success(user);
    }
}