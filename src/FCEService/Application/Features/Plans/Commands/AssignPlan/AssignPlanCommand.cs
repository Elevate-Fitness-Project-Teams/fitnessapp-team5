using FCEService.Domain.Common.Results;
using MediatR;
using System;

namespace FCEService.Application.Features.Plans.Commands.AssignPlan;

public sealed record AssignPlanCommand(Guid UserId) : IRequest<Result<Unit>>;
