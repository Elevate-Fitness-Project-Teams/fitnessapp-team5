
using NotificationService.Abstractions;

namespace NotificationService.Errors;

public static class NotificationErrors
{
    public static Error NotFound(int notificationId) =>
        new("Notification.NotFound", $"Notification with ID {notificationId} was not found.", StatusCodes.Status404NotFound);
}
