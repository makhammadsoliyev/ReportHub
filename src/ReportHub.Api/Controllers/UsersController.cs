using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.Users.LoginUser;
using ReportHub.Application.Users.RegisterUser;

namespace ReportHub.Api.Controllers;

public class UsersController(ISender mediator) : BaseController(mediator)
{
	[AllowAnonymous]
	[HttpPost("register")]
	public async Task<IActionResult> LoginAsync([FromBody] RegisterUserCommand command)
	{
		var result = await Mediator.Send(command);

		return Ok(result);
	}

	[AllowAnonymous]
	[HttpPost("login")]
	public async Task<IActionResult> LoginAsync([FromBody] LoginUserCommand command)
	{
		var result = await Mediator.Send(command);

		return Ok(result);
	}

	[HttpGet("health-check")]
	public ActionResult<string> HealthCheck()
	{
		return Ok("This application is developed by Umidbek Maxammadsoliyev");
	}
}
