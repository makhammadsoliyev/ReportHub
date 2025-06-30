namespace ReportHub.Domain.Common;

public abstract class SoftDeletableAuditableEntity : AuditableEntity, ISoftDeletable
{
	public bool IsDeleted { get; set; }

	public DateTime? DeletedOnUtc { get; set; }

	public Guid? DeletedBy { get; set; }
}
