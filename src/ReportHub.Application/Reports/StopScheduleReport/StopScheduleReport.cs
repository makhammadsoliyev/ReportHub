using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Reports.StopScheduleReport;

public class StopScheduleReportCommand(Guid organizationId) : ICommand<bool>, IOrganizationRequest
{
	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class StopScheduleReportCommandHandler(ICurrentUserService currentUserService, IQuartzJobService quartzJobService)
	: ICommandHandler<StopScheduleReportCommand, bool>
{
	public async Task<bool> Handle(StopScheduleReportCommand request, CancellationToken cancellationToken)
	{
		var result = await quartzJobService.StopAsync(currentUserService.UserId);

		return result;
	}
}
