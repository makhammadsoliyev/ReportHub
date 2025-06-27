using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Services;

public interface ITokenService
{
	Task<string> GenerateAccessTokenAsync(User user);
}
