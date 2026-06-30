namespace FCEService.Domain.Common;

public abstract class AuditableEntity : Entity
{
    protected AuditableEntity()
    { }

    protected AuditableEntity(Guid id)
        : base(id)
    {
    }

    public DateTimeOffset CreatedAtUtc { get; init; }

    public string? CreatedBy { get; init; }

    public DateTimeOffset LastModifiedUtc { get; set; }

    public string? LastModifiedBy { get; set; }
}