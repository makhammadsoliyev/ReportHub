namespace ReportHub.Domain.Common;

public abstract class SoftDeletableAuditableEntity : IEntity, IAuditable, ISoftDeletable
{
	public Guid Id { get; set; }

	public Guid CreatedBy { get; set; }

	public DateTime CreatedOnUtc { get; set; }

	public Guid? UpdatedBy { get; set; }

	public DateTime? UpdatedOnUtc { get; set; }

	public bool IsDeleted { get; set; }

	public DateTime? DeletedOnUtc { get; set; }

	public Guid? DeletedBy { get; set; }
}
