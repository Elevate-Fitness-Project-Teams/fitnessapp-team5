using System;
using FCEService.Domain.Common.Results;
using MediatR;

namespace FCEService.Application.Features.Plans.Queries.GetUserAssignedPlan;

public sealed record GetUserAssignedPlanQuery(Guid UserId) : IRequest<Result<GetUserAssignedPlanResponse>>;
