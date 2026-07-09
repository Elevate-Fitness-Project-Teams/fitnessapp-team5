using System;
using FCEService.Domain.Common.Results;
using MediatR;

namespace FCEService.Application.Features.Plans.Queries.GetPlanConfigById;

public sealed record GetPlanConfigByIdQuery(Guid PlanId) : IRequest<Result<GetPlanConfigByIdResponse>>;
