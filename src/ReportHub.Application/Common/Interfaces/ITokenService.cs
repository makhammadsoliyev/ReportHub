using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces;

public interface ITokenService
{
	Task<string> GenerateAccessTokenAsync(User user);
}
