using MediatR;
using FCEService.Domain.Common.Results;
using FCEService.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FCEService.Domain.Entities.UserFitnessStats;

namespace FCEService.Application.Features.Biometrics.Queries.GetBiometrics;

public sealed class GetBiometricsByIdQueryHandler(IAppDbContext dbContext, ILogger<GetBiometricsByIdQueryHandler> logger) : IRequestHandler<GetBiometricsByIdQuery, Result<GetBiometricsByIdResponse>>
{
    public async Task<Result<GetBiometricsByIdResponse>> Handle(
      GetBiometricsByIdQuery query,
       CancellationToken ct)
    {
        // LoggingBehaviour already logs every request — no need to log again here.
        var userFitnessStats = await dbContext.UserFitnessStats
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == query.UserId, ct);

        if (userFitnessStats is null)
        {
            logger.LogWarning("UserFitnessStats not found for UserId: {UserId}", query.UserId);
            return UserFitnessStatsErrors.UserNotFound;
        }

        return userFitnessStats.ToResponse();
    }
}
