namespace ReportHub.Application.Common.Interfaces.Services;

public interface IQuartzJobService
{
	Task ScheduleAsync(Guid userId, string email, TimeSpan interval, Guid organizationId);

	Task<bool> StopAsync(Guid userId);
}
