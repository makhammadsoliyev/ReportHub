using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.Invoices.CreateInvoice;

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
}
