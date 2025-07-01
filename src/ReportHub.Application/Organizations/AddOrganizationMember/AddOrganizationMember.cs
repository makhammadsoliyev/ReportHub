using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Organizations.AddOrganizationMember;

public class AddOrganizationMemberCommand : ICommand<Guid>, IOrganizationRequest
{
	public Guid UserId { get; set; }

	public Guid RoleId { get; set; }

	public Guid OrganizationId { get; set; }
}
