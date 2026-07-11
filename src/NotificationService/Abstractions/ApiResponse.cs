namespace NotificationService.Abstractions;

public record ApiResponse(
    bool IsSuccess,
    string Message,
    object? Data,
    IList<Error> Errors,
    int? StatusCode,
    DateTime Timestamp
);