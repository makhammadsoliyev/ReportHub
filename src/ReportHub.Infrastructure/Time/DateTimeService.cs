using ReportHub.Application.Common.Interfaces.Services;

namespace ReportHub.Infrastructure.Time;

public class DateTimeService : IDateTimeService
{
	public DateTime UtcNow => DateTime.UtcNow;
}
