namespace ReportHub.Application.Organizations.GetOrganizationMembersList;

public class GetOrganizationMembersListDto
{
	public Guid Id { get; set; }

	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string Email { get; set; }

	public string OrganizationRole { get; set; }
}
