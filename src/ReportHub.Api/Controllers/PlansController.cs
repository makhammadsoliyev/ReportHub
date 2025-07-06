using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.Plans.AddPlanItem;
using ReportHub.Application.Plans.CreatePlan;
using ReportHub.Application.Plans.DeletePlan;
using ReportHub.Application.Plans.DeletePlanItem;
using ReportHub.Application.Plans.GetPlanById;
using ReportHub.Application.Plans.GetPlanItemsList;
using ReportHub.Application.Plans.GetPlansList;
using ReportHub.Application.Plans.UpdatePlan;

namespace ReportHub.Api.Controllers;

[Route("organizations/{organizationId:guid}/plans")]
public class PlansController(ISender mediator) : BaseController(mediator)
{
	[HttpPost]
	public async Task<IActionResult> CreateAsync([FromBody] CreatePlanRequest request, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new CreatePlanCommand(request, organizationId));

		return Ok(result);
	}

	[HttpPut]
	public async Task<IActionResult> UpdateAsync([FromBody] UpdatePlanRequest request, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new UpdatePlanCommand(request, organizationId));

		return Ok(result);
	}

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteAsync([FromRoute] Guid id, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new DeletePlanCommand(id, organizationId));

		return Ok(result);
	}

	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new GetPlanByIdQuery(id, organizationId));

		return Ok(result);
	}

	[HttpGet]
	public async Task<IActionResult> GetListAsync([FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new GetPlansListQuery(organizationId));

		return Ok(result);
	}

	[HttpPost("{id:guid}/items")]
	public async Task<IActionResult> AddItemAsync(
		[FromBody] AddPlanItemRequest request, [FromRoute] Guid id, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new AddPlanItemCommand(id, request, organizationId));

		return Ok(result);
	}

	[HttpDelete("{id:guid}/items/{planItemId:guid}")]
	public async Task<IActionResult> DeleteItemAsync([FromRoute] Guid id, [FromRoute] Guid planItemId, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new DeletePlanItemCommand(planItemId, id, organizationId));

		return Ok(result);
	}

	[HttpGet("{id:guid}/items")]
	public async Task<IActionResult> GetItemsListAsync([FromRoute] Guid id, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new GetPlanItemsListQuery(id, organizationId));

		return Ok(result);
	}
}
