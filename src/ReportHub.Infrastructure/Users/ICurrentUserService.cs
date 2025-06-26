using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using ReportHub.Application.Common.Interfaces;

namespace ReportHub.Infrastructure.Users;

public class CurrentUserService(IHttpContextAccessor httpContextAccessor) : ICurrentUserService
{
	public Guid UserId => httpContextAccessor.HttpContext.User.GetUserId();
}

public static class ClaimsPrincipalExtensions
{
	public static Guid GetUserId(this ClaimsPrincipal principal)
	{
		var userId = principal?.FindFirstValue(ClaimTypes.NameIdentifier);

		return Guid.TryParse(userId, out var parsedUserId) ? parsedUserId : Guid.Empty;
	}
}