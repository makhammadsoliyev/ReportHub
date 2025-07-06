using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Users.LoginUserWithRefreshToken;

public class LoginUserWithRefreshTokenCommand(string refreshToken) : ICommand<LoginUserWithRefreshTokenDto>
{
	public string RefreshToken { get; set; } = refreshToken;
}

public class LoginUserWithRefreshTokenCommandHandler(ITokenService tokenService, IDateTimeService dateTimeService, IRefreshTokenRepository refreshTokenRepository)
	: ICommandHandler<LoginUserWithRefreshTokenCommand, LoginUserWithRefreshTokenDto>
{
	public async Task<LoginUserWithRefreshTokenDto> Handle(LoginUserWithRefreshTokenCommand request, CancellationToken cancellationToken)
	{
		var refreshToken = await refreshTokenRepository.SelectAsync(t => t.Token == request.RefreshToken);

		if (refreshToken is null || refreshToken.ExpiresOnUtc < dateTimeService.UtcNow)
		{
			throw new UnauthorizedException("The refresh token has expired");
		}

		var accessToken = await tokenService.GenerateAccessTokenAsync(refreshToken.User);

		refreshToken.Token = tokenService.GenerateRefreshToken();
		refreshToken.ExpiresOnUtc = dateTimeService.UtcNow.AddDays(7);

		await refreshTokenRepository.UpdateAsync(refreshToken);

		return new LoginUserWithRefreshTokenDto
		{
			AccessToken = accessToken,
			RefreshToken = refreshToken.Token,
		};
	}
}
