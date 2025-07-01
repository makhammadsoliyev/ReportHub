namespace ReportHub.Application.Organizations.AddOrganizationMember;

public class AddOrganizationMemberRequest
{
	public Guid UserId { get; set; }

	public Guid RoleId { get; set; }
}
