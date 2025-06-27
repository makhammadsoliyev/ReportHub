using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.Users.GetUserByEmail;
using ReportHub.Application.Users.GetUserList;
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

	[HttpGet]
	public async Task<IActionResult> GetListAsync()
	{
		var result = await Mediator.Send(new GetUserListQuery());

		return Ok(result);
	}

	[HttpGet("{email}")]
	public async Task<IActionResult> GetByEmailAsync([FromRoute] string email)
	{
		var result = await Mediator.Send(new GetUserByEmailQuery(email));

		return Ok(result);
	}
}
