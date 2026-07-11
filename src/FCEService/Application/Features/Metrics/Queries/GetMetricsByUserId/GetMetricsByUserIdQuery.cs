using FCEService.Domain.Common.Results;
using MediatR;

namespace FCEService.Application.Features.Metrics.Queries.GetMetricsByUserId;

public sealed record GetMetricsByUserIdQuery(Guid UserId) : IRequest<Result<GetMetricsByUserIdResponse>>;
