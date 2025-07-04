using AutoMapper;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.ExportLogs.GetLogsList;

public class GetLogsListQuery(GetLogsListFilter filter, Guid organizationId)
	: IQuery<IEnumerable<GetLogsListDto>>, IOrganizationRequest
{
	public GetLogsListFilter Filter { get; set; } = filter;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class GetLogsListQueryHandler(IMapper mapper, ILogRepository repository) :
	IQueryHandler<GetLogsListQuery, IEnumerable<GetLogsListDto>>
{
	public async Task<IEnumerable<GetLogsListDto>> Handle(GetLogsListQuery request, CancellationToken cancellationToken)
	{
		var logs = await repository.SelectAllAsync();

		if (request.Filter.StartDate is DateTime startDate)
		{
			logs = logs.FindAll(log => startDate <= log.TimeStamp);
		}

		if (request.Filter.EndDate is DateTime endDate)
		{
			logs = logs.FindAll(log => log.TimeStamp <= endDate);
		}

		return mapper.Map<IEnumerable<GetLogsListDto>>(logs);
	}
}
