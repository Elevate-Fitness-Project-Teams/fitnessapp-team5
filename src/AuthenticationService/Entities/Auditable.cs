namespace AuthenticationService.Entities
{
    public abstract class Auditable
    {
        public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; protected set; }
        public Guid? UpdatedBy { get; protected set; }
    }
}
