using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.Items.CreateItem;
using ReportHub.Application.Items.DeleteItem;
using ReportHub.Application.Items.GetItemById;
using ReportHub.Application.Items.GetItemsList;
using ReportHub.Application.Items.UpdateItem;

namespace ReportHub.Api.Controllers;

[Route("organizations/{organizationId:guid}/items")]
public class ItemsController(ISender mediator) : BaseController(mediator)
{
	[HttpPost]
	public async Task<IActionResult> CreateAsync([FromBody] CreateItemRequest request, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new CreateItemCommand(request, organizationId));

		return Ok(result);
	}

	[HttpPut]
	public async Task<IActionResult> UpdateAsync([FromBody] UpdateItemRequest request, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new UpdateItemCommand(request, organizationId));

		return Ok(result);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new DeleteItemCommand(id, organizationId));

		return Ok(result);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new GetItemByIdQuery(id, organizationId));

		return Ok(result);
	}

	[HttpGet]
	public async Task<IActionResult> GetListAsync([FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new GetItemsListQuery(organizationId));

		return Ok(result);
	}
}
