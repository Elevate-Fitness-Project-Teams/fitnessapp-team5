using MediatR;
using FCEService.Domain.Common.Results;
using System;

namespace FCEService.Application.Features.Biometrics.Queries.GetBiometrics;

public sealed record GetBiometricsByIdQuery(Guid UserId) : IRequest<Result<GetBiometricsByIdResponse>>;