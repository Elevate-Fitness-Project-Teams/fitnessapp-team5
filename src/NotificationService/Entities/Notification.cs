using NotificationService.Entities.Enum;

namespace NotificationService.Entities;

public class Notification
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public NotificationType Type { get; set; }
    public bool IsRead { get; set; }
    public DateTime SentAt { get; set; }

    private Notification() { }
}