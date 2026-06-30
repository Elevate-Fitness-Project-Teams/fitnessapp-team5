namespace AuthenticationService.Entities
{
    public class RefreshToken
    {
        public Guid Id { get; set; }
        public string Token { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;
        public bool IsRevoked { get; private set; }
        public DateTime? RevokedAt { get; private set; }

        // Relationship with user
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        // Business Rules
        public void Revoke()
        {
            IsRevoked = true;
            RevokedAt = DateTime.UtcNow;
        }
    }
}
