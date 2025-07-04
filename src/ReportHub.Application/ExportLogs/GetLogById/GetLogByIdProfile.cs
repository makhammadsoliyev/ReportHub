using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.ExportLogs.GetLogById;

public class GetLogByIdProfile : Profile
{
	public GetLogByIdProfile()
	{
		CreateMap<Log, GetLogByIdDto>();
	}
}
