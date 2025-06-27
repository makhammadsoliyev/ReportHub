using ReportHub.Domain.Common;

namespace ReportHub.Domain;

public class Organization : SoftDeletableAuditableEntity
{
	public string Name { get; set; }

	public string CountryCode { get; set; }
}
