using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.Organizations.AddOrganizationMember;
using ReportHub.Application.Organizations.CreateOrganization;
using ReportHub.Application.Organizations.GetOrganizationMembersList;
using ReportHub.Application.Organizations.GetOrganizationRolesList;

namespace ReportHub.Api.Controllers;

[Route("[controller]")]
public class OrganizationsController(ISender mediator) : BaseController(mediator)
{
	[HttpPost]
	public async Task<IActionResult> CreateAsync([FromBody] CreateOrganizationCommand command)
	{
		var result = await Mediator.Send(command);

		return Ok(result);
	}

	[HttpGet("roles")]
	public async Task<IActionResult> GetRolesListAsync()
	{
		var result = await Mediator.Send(new GetOrganizationRolesListQuery());

		return Ok(result);
	}

	[HttpPost("{id:guid}/members")]
	public async Task<IActionResult> AddMemberAsync([FromRoute] Guid id, [FromBody] AddOrganizationMemberRequest request)
	{
		var result = await Mediator.Send(new AddOrganizationMemberCommand(request, id));

		return Ok(result);
	}

	[HttpGet("{id:guid}/members")]
	public async Task<IActionResult> GetMembersListAsync([FromRoute] Guid id)
	{
		var result = await Mediator.Send(new GetOrganizationMembersListQuery(id));

		return Ok(result);
	}
}
