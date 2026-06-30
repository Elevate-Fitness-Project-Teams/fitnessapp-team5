using FCEService.Domain.Common.Results;
using MediatR;
using FCEService.Domain.Enums; // Assuming you have these enums defined

namespace FCEService.Application.Features.Biometrics.Commands.IngestBiometrics;

public sealed record IngestBiometricsCommand(
    Guid UserId,
    double Weight,
    double Height,
    DateTime BirthDate,
    Gender Gender,
    Goal Goal,
    ActivityLevel ActivityLevel
) : IRequest<Result<Success>>; 
