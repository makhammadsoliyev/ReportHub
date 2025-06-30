using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.Customers.CreateCustomer;
using ReportHub.Application.Customers.DeleteCustomer;
using ReportHub.Application.Customers.GetCustomerById;
using ReportHub.Application.Customers.GetCustomersList;
using ReportHub.Application.Customers.UpdateCustomer;

namespace ReportHub.Api.Controllers;

[Route("organizations/{organizationId:guid}/[controller]")]
public class CustomersController(ISender mediator) : BaseController(mediator)
{
	[HttpPost]
	public async Task<IActionResult> CreateAsync([FromBody] CreateCustomerRequest request, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new CreateCustomerCommand(request, organizationId));

		return Ok(result);
	}

	[HttpPut]
	public async Task<IActionResult> UpdateAsync([FromBody] UpdateCustomerRequest request, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new UpdateCustomerCommand(request, organizationId));

		return Ok(result);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new GetCustomerByIdQuery(id, organizationId));

		return Ok(result);
	}

	[HttpGet]
	public async Task<IActionResult> GetListAsync([FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new GetCustomersListQuery(organizationId));

		return Ok(result);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new DeleteCustomerCommand(id, organizationId));

		return Ok(result);
	}
}
