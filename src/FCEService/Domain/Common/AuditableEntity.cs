namespace FCEService.Domain.Common;

public abstract class AuditableEntity : Entity
{
    protected AuditableEntity()
    { }

    protected AuditableEntity(Guid id)
        : base(id)
    {
    }

    public DateTimeOffset CreatedAtUtc { get; protected set; }

    public string CreatedBy { get; protected set; } = string.Empty;

    public DateTimeOffset LastModifiedUtc { get; protected set; }

    public string? LastModifiedBy { get; protected set; }
}