using Microsoft.AspNetCore.Identity;
using ReportHub.Domain.Common;

namespace ReportHub.Domain;

public class User : IdentityUser<Guid>, IEntity, IAuditable, ISoftDeletable
{
	public string FirstName { get; set; }

	public string LastName { get; set; }

	public Guid CreatedBy { get; set; }

	public DateTime CreatedOnUtc { get; set; }

	public Guid? UpdatedBy { get; set; }

	public DateTime? UpdatedOnUtc { get; set; }

	public bool IsDeleted { get; set; }

	public DateTime? DeletedOnUtc { get; set; }

	public Guid? DeletedBy { get; set; }
}
