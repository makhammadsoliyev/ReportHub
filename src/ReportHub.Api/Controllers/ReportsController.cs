using MediatR;
using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.Reports.ExportReport;
using ReportHub.Application.Reports.ScheduleReport;

namespace ReportHub.Api.Controllers;

[Route("organizations/{organizationId:guid}/reports")]
public class ReportsController(ISender mediator) : BaseController(mediator)
{
	[HttpPost]
	public async Task<IActionResult> ScheduleAsync([FromRoute] Guid organizationId, TimeSpan interval)
	{
		var result = await Mediator.Send(new ScheduleReportCommand(organizationId, interval));

		return Ok(result);
	}

	[HttpGet("export")]
	public async Task<IActionResult> ExportAsync([FromRoute] Guid organizationId)
	{
		var result = await Mediator.Send(new ExportReportQuery(organizationId));

		return File(result.Content, result.ContentType, result.FileName);
	}
}
