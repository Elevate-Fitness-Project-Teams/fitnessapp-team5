using FCEService.Application.Common.Interfaces;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Entities.UserFitnessStats;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FCEService.Application.Features.Biometrics.Queries.GetBiometrics;

public class GetBiometricsByIdQueryHandler(IAppDbContext dbContext, ILogger<GetBiometricsByIdQueryHandler> logger)
    : IRequestHandler<GetBiometricsByIdQuery, Result<GetBiometricsByIdResponse>>
{
    public async Task<Result<GetBiometricsByIdResponse>> Handle(GetBiometricsByIdQuery query, CancellationToken ct)
    {
        logger.LogInformation("Getting UserFitnessStats for this UserId:{UserId}", query.UserId);

        var userFitnessStats = await dbContext.UserFitnessStats.FirstOrDefaultAsync(x => x.UserId == query.UserId, ct);

        if (userFitnessStats == null)
        {
            logger.LogWarning("UserFitnessStats for this UserId:{UserId} not found", query.UserId);
            return UserFitnessStatsErrors.UserNotFound;
        }
        logger.LogInformation("UserFitnessStats for this UserId:{UserId} found", query.UserId);
        return userFitnessStats.ToResponse();
    }
}