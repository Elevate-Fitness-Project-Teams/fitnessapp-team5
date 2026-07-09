using FCEService.Domain.Common.Results;
using FCEService.Domain.Enums;
using MediatR;

namespace FCEService.Application.Features.Biometrics.Commands.RegisterUserFitness;


public sealed record RegisterUserFitnessCommand(
    Guid UserId,
    double Weight,
    double Height,
    DateTime BirthDate,
    Gender Gender,
    Goal Goal,
    ActivityLevel ActivityLevel
) : IRequest<Result<RegisterUserFitnessResponse>>;
