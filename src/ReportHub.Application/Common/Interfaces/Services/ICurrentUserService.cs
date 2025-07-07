namespace ReportHub.Application.Common.Interfaces.Services;

public interface ICurrentUserService
{
	Guid UserId { get; }

	string[] UserRoles { get; }

	string Email { get; }
}
