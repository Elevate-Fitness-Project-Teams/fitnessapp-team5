namespace NotificationService.Contracts.Notifications;

public record NotificationResponse(
  Guid UserId,
  string Title,
  string Message,
  string Type,
  DateTime SentAt
);