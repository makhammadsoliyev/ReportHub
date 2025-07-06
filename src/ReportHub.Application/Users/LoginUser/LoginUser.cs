using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;
using ReportHub.Domain;

namespace ReportHub.Application.Users.LoginUser;

public class LoginUserCommand(string email, string password) : ICommand<LoginUserDto>
{
	public string Email { get; set; } = email;

	public string Password { get; set; } = password;
}

public class LoginUserCommandHandler(
	ITokenService tokenService,
	IIdentityService identityService,
	IDateTimeService dateTimeService,
	IRefreshTokenRepository refreshTokenRepository)
	: ICommandHandler<LoginUserCommand, LoginUserDto>
{
	public async Task<LoginUserDto> Handle(LoginUserCommand request, CancellationToken cancellationToken)
	{
		var user = await identityService.LoginAsync(request.Email, request.Password);
		if (!user.EmailConfirmed)
		{
			throw new UnauthorizedException("Email is not confirmed");
		}

		var accessToken = await tokenService.GenerateAccessTokenAsync(user);
		var refreshToken = tokenService.GenerateRefreshToken();

		var token = new RefreshToken
		{
			UserId = user.Id,
			Token = refreshToken,
			ExpiresOnUtc = dateTimeService.UtcNow.AddDays(7),
		};

		await refreshTokenRepository.InsertAsync(token);

		return new LoginUserDto
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken,
		};
	}
}
