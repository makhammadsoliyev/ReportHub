using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Users.LoginUser;

public class LoginUserCommand(string email, string password) : ICommand<LoginUserDto>
{
	public string Email { get; set; } = email;

	public string Password { get; set; } = password;
}

public class LoginUserCommandHandler(ITokenService tokenService, IIdentityService identityService)
	: ICommandHandler<LoginUserCommand, LoginUserDto>
{
	public async Task<LoginUserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
	{
		var user = await identityService.LoginAsync(request.Email, request.Password);
		var accessToken = await tokenService.GenerateAccessTokenAsync(user);

		return new LoginUserDto
		{
			AccessToken = accessToken,
		};
	}
}
