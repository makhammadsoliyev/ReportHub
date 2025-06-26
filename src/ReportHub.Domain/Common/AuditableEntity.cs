namespace ReportHub.Domain.Common;

public abstract class AuditableEntity : IEntity, IAuditable
{
	public Guid Id { get; set; }

	public Guid CreatedBy { get; set; }

	public DateTime CreatedOnUtc { get; set; }

	public Guid? UpdatedBy { get; set; }

	public DateTime? UpdatedOnUtc { get; set; }
}
