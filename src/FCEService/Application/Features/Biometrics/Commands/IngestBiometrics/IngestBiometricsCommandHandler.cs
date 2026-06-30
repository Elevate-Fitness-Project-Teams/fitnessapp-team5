namespace FCEService.Application.Features.Biometrics.Commands.IngestBiometrics;

using FCEService.Application.Common.Behaviours;
using FCEService.Domain.Common.Results;
using FCEService.Domain.Entities.UserFitnessStats;
using MediatR;
using FCEService.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;

public class BiometricsCommandHandler(ILogger<IngestBiometricsCommand> logger , IAppDbContext context )   :IRequestHandler<IngestBiometricsCommand,Result<Success>>
{
    public async Task<Result<Success>> Handle(IngestBiometricsCommand command, CancellationToken ct)
    {
      var userFitnessStats = await context.UserFitnessStats.FirstOrDefaultAsync(x => x.UserId == command.UserId, ct);

      if(userFitnessStats != null)
      {
       logger.LogInformation("UserFitnessStats for this UserId:{UserId} already exists", command.UserId);
       return UserFitnessStatsErrors.UserAlreadyExists;
      }
        var entity = UserFitnessStats.Create(command.UserId, command.Weight, command.Height, command.BirthDate, command.Gender, command.Goal, command.ActivityLevel, "System");
        if(entity.IsError)
        {
            return entity.Errors;
        }
        await context.UserFitnessStats.AddAsync(entity.Value, ct);
       // userFitnessStats.AddEvent(new UserFitnessStatsCreatedDomainEvent(entity.Value));
        await context.SaveChangesAsync(ct);
        return Result.Success;
 
    }
}