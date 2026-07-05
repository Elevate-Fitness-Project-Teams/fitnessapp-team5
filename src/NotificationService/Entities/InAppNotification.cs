using NotificationService.Entities.Enum;

namespace NotificationService.Entities;

public class InAppNotification
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime SentAt { get; set; }

    private InAppNotification() { }

    public static InAppNotification Create(Guid userId, string title, string message, NotificationType type) =>
        new()
        {
            UserId = userId,
            Title = title,
            Message = message,
            Type = type,
            IsRead = false,
            SentAt = DateTime.UtcNow
        };

    public void MarkAsRead()
    {
        if (!IsRead)
            IsRead = true;
    }
}