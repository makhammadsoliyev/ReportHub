using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ReportHub.Api.Controllers;

[ApiController]
[Authorize]
public class BaseController : ControllerBase
{
	protected BaseController(ISender mediator)
	{
		Mediator = mediator;
	}

	protected ISender Mediator { get; }
}
