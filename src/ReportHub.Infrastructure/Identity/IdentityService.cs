using Microsoft.AspNetCore.Identity;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Identity;

public class IdentityService(UserManager<User> userManager) : IIdentityService
{
	public async Task RegisterAsync(User user, string password)
	{
		user.UserName = user.Email;

		var identityResult = await userManager.CreateAsync(user, password);

		if (!identityResult.Succeeded)
			throw new UnauthorizedException(
				identityResult.Errors
					.Select(error => error.Description)
					.Aggregate((previous, next) => previous + "\n" + next));
	}

	public async Task<User> LoginAsync(string email, string password)
	{
		var user = await userManager.FindByEmailAsync(email)
			?? throw new UnauthorizedException("Invalid email or password");

		var identityResult = await userManager.CheckPasswordAsync(user, password);

		if (!identityResult)
			throw new UnauthorizedException("Invalid email or password");

		return user;
	}
}
