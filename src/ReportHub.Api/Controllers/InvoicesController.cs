using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.Invoices.CreateInvoice;
using ReportHub.Application.Invoices.DeleteInvoice;
using ReportHub.Application.Invoices.GetInvoiceById;
using ReportHub.Application.Invoices.GetInvoicesList;
using ReportHub.Application.Invoices.UpdateInvoice;

namespace ReportHub.Api.Controllers;

[Route("organizations/{organizationId:guid}/[controller]")]
public class InvoicesController(ISender mediator) : BaseController(mediator)
{
	[HttpPost]
	public async Task<IActionResult> CreateAsync([FromBody] CreateInvoiceRequest request, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new CreateInvoiceCommand(request, organizationId));

		return Ok(result);
	}

	[HttpPut]
	public async Task<IActionResult> UpdateAsync([FromBody] UpdateInvoiceRequest request, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new UpdateInvoiceCommand(request, organizationId));

		return Ok(result);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new DeleteInvoiceCommand(id, organizationId));

		return Ok(result);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, [FromRoute,] Guid organizationId)
	{
		var result = await Mediator.Send(new GetInvoiceByIdQuery(id, organizationId));

		return Ok(result);
	}

	[HttpGet]
	public async Task<IActionResult> GetListAsync([FromRoute,] Guid organizationId)
	{
		var result = await Mediator.Send(new GetInvoicesListQuery(organizationId));

		return Ok(result);
	}
}
