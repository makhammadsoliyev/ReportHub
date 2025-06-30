using ReportHub.Application.Common.Interfaces.Services;

namespace ReportHub.Infrastructure.Organizations;

public class CurrentOrganizationService : ICurrentOrganizationService
{
	public Guid OrganizationId { get; set; }
}
