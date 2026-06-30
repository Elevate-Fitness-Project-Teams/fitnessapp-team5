namespace AuthenticationService.Entities
{
    public sealed class ResetToken : Auditable
    {
        public Guid Id { get; private set; }
        public string TokenHash { get; private set; } = default!;
        public DateTime ExpiresAt { get; private set; }
        public bool IsUsed { get; private set; }

        /////////////////////////////////////////////////////

        public Guid UserId { get; private set; }
        public User User { get; private set; } = default!;

        /////////////////////////////////////////////////////

        public static ResetToken Create(Guid userId, string tokenHash, DateTime expiresAt)
        {
            return new ResetToken
            {
                Id = Guid.CreateVersion7(),
                UserId = userId,
                TokenHash = tokenHash,
                ExpiresAt = expiresAt,
                IsUsed = false
            };
        }

        public void MarkAsUsed()
        {
            IsUsed = true;
        }
    }
}
