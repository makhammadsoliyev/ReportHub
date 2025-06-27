using ReportHub.Domain.Common;

namespace ReportHub.Domain;

public class Customer : SoftDeletableAuditableEntity
{
	public string Name { get; set; }

	public string Email { get; set; }

	public string CountryCode { get; set; }

	public Guid OrganizationId { get; set; }

	public Organization Organization { get; set; }
}
