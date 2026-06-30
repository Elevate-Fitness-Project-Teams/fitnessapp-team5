namespace AuthenticationService.Entities
{
    public class LoginAttempt
    {
        public Guid Id { get; set; }
        public string Email { get; set; } = default!;
        public bool IsSuccessful { get; set; }
        public string? FailureReason { get; set; }
        public string? IpAddress { get; set; }
        public string? UserAgent { get; set; }

        public DateTime AttemptedAt { get; set; } = DateTime.UtcNow;

        // Relationship with user
        public Guid? UserId { get; set; }
        public User? User { get; set; } = default!;
    }
}
