using Mapster;
using MediatR;
using NotificationService.Abstractions;
using NotificationService.Contracts.Notifications;
using NotificationService.Entities;
using NotificationService.InterFaces;

namespace NotificationService.Features.Notifications.GetNotifications;

public record GetNotificationsQuery(RequestFilters Filters) : IRequest<PaginatedList<NotificationResponse>>;

public class GetNotificationsQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetNotificationsQuery, PaginatedList<NotificationResponse>>
{
    public async Task<PaginatedList<NotificationResponse>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var source = unitOfWork
            .Repository<Notification>()
            .GetQueryable()
            .Where(x => !x.IsRead)
            .ProjectToType<NotificationResponse>();

        return await PaginatedList<NotificationResponse>
            .CreateAsync(source, request.Filters.PageNumber, request.Filters.PageSize, cancellationToken);
    }
}
