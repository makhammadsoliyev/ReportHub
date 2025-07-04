using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.ExportLogs.GetLogById;
using ReportHub.Application.ExportLogs.GetLogsList;

namespace ReportHub.Api.Controllers;

[Route("organizations/{organizationId:guid}/[controller]")]
public class LogsController(ISender mediator) : BaseController(mediator)
{
	[HttpGet("{id:guid}")]
	public async Task<IActionResult> GetAsync([FromRoute] Guid id, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new GetLogByIdQuery(id, organizationId));

		return Ok(result);
	}

	[HttpGet]
	public async Task<IActionResult> GetListAsync([FromQuery] GetLogsListFilter filter, [FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new GetLogsListQuery(filter, organizationId));

		return Ok(result);
	}
}
