using ReportHub.Domain.Common;

namespace ReportHub.Domain;

public class OrganizationMember : SoftDeletableAuditableEntity
{
	public Guid UserId { get; set; }

	public User User { get; set; }

	public Guid OrganizationId { get; set; }

	public Organization Organization { get; set; }

	public Guid OrganizationRoleId { get; set; }

	public OrganizationRole OrganizationRole { get; set; }
}
