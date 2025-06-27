using ReportHub.Domain.Common;

namespace ReportHub.Domain;

public class OrganizationRole : SoftDeletableAuditableEntity
{
	public string Name { get; set; }
}
