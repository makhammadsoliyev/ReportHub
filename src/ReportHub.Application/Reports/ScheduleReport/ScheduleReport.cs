using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Reports.ScheduleReport;

public class ScheduleReportCommand(Guid organizationId, TimeSpan interval) : ICommand<bool>, IOrganizationRequest
{
	public TimeSpan Interval { get; set; } = interval;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class ScheduleReportCommandHandler(ICurrentUserService currentUserService, IQuartzJobService quartzJobService)
	: ICommandHandler<ScheduleReportCommand, bool>
{
	public async Task<bool> Handle(ScheduleReportCommand request, CancellationToken cancellationToken)
	{
		await quartzJobService.ScheduleAsync(currentUserService.UserId, currentUserService.Email, request.Interval, request.OrganizationId);

		return true;
	}
}
