using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.ExportLogs.GetLogsList;

public class GetLogsListProfile : Profile
{
	public GetLogsListProfile()
	{
		CreateMap<Log, GetLogsListDto>();
	}
}
