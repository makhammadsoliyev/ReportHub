using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReportHub.Application.Users.ConfirmUserEmail;
using ReportHub.Application.Users.DeleteUser;
using ReportHub.Application.Users.GetUserByEmail;
using ReportHub.Application.Users.GetUserList;
using ReportHub.Application.Users.LoginUser;
using ReportHub.Application.Users.RegisterUser;

namespace ReportHub.Api.Controllers;

[Route("[controller]")]
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

	[AllowAnonymous]
	[HttpPatch("email-confirmation")]
	public async Task<IActionResult> ConfirmEmailAsync([FromQuery] Guid id, [FromQuery] string token)
	{
		var result = await Mediator.Send(new ConfirmUserEmailCommand(id, token));

		return Ok(result);
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

	[HttpDelete("{id:guid}")]
	public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
	{
		var result = await Mediator.Send(new DeleteUserCommand(id));

		return Ok(result);
	}
}
