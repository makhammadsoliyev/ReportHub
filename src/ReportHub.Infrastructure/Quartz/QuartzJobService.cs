using Quartz;
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

		await scheduler.ScheduleJob(job, trigger);
	}
}
