using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.Organizations.CreateOrganization;

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
}
