using Quartz;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Services;

namespace ReportHub.Infrastructure.Quartz;

public class QuartzJobService(ISchedulerFactory schedulerFactory) : IQuartzJobService
{
	public async Task ScheduleAsync(Guid userId, string email, TimeSpan interval, Guid organizationId)
	{
		var scheduler = await schedulerFactory.GetScheduler();

		var job = JobBuilder.Create<ReportEmailSenderJob>()
			.WithIdentity($"{ReportEmailSenderJob.Name}_{userId}")
			.UsingJobData(nameof(userId), userId)
			.UsingJobData(nameof(email), email)
			.UsingJobData(nameof(organizationId), organizationId)
			.Build();

		var trigger = TriggerBuilder.Create()
			.WithIdentity($"{ReportEmailSenderJob.Name}_{userId}")
			.ForJob(job)
			.WithSimpleSchedule(t => t.WithInterval(interval).RepeatForever())
			.Build();
		try
		{
			await scheduler.ScheduleJob(job, trigger);
		}
		catch (ObjectAlreadyExistsException exception)
		{
			throw new ConflictException(exception.Message);
		}
	}

	public async Task<bool> StopAsync(Guid userId)
	{
		var scheduler = await schedulerFactory.GetScheduler();

		return await scheduler.DeleteJob(new JobKey($"{ReportEmailSenderJob.Name}_{userId}"));
	}
}
