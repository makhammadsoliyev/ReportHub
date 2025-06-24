using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces;

public interface IIdentityService
{
	Task RegisterAsync(User user, string password);

	Task<User> LoginAsync(string email, string password);
}
