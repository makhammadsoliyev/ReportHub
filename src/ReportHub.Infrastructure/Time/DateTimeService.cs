using ReportHub.Application.Common.Interfaces;

namespace ReportHub.Infrastructure.Time;

public class DateTimeService : IDateTimeService
{
	public DateTime UtcNow => DateTime.UtcNow;
}
