namespace AuthenticationService.Entities
{
    public class OtpCode
    {
        public Guid Id { get; set; }
        public string CodeHash { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime ExpiresAt { get; set; }
        public bool IsUsed { get; private set; }
        public DateTime? UsedAt { get; private set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresAt;

        ///////////////////////////////////
        public Guid UserId { get; set; }
        public User User { get; set; } = default!;

        /////////////////////////////////////
        public void MarkAsUsed()
        {
            IsUsed = true;
            UsedAt = DateTime.UtcNow;
        }
    }
}
