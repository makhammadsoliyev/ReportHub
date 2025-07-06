using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Invoices.GetLogsList;

public class GetLogsListProfile : Profile
{
	public GetLogsListProfile()
	{
		CreateMap<Log, GetLogsListDto>();
	}
}
