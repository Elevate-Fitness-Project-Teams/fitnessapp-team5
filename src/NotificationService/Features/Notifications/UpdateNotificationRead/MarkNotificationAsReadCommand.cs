using MediatR;
using Microsoft.EntityFrameworkCore;
using NotificationService.Abstractions;
using NotificationService.Entities;
using NotificationService.Errors;
using NotificationService.InterFaces;
using NotificationService.InterFaces.Services;

namespace NotificationService.Features.Notifications.UpdateNotificationRead;

public record MarkNotificationAsReadCommand(int Id) : IRequest<Result>;

public class MarkNotificationAsReadCommandHandler(IUnitOfWork unitOfWork, ICurrentUser currentUser)
    : IRequestHandler<MarkNotificationAsReadCommand, Result>
{
    public async Task<Result> Handle(MarkNotificationAsReadCommand request, CancellationToken cancellationToken)
    {
        var rowsAffected = await unitOfWork
            .Repository<Notification>()
            .GetQueryable()
            .Where(n => n.Id == request.Id && n.UserId == currentUser.Id)
            .ExecuteUpdateAsync(s => s.SetProperty(n => n.IsRead, true), cancellationToken);

        if (rowsAffected == 0)
            return Result.Failure(NotificationErrors.NotFound(request.Id));

        return Result.Success();
    }
}