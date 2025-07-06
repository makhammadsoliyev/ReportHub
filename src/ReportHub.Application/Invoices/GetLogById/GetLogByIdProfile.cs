using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Invoices.GetLogById;

public class GetLogByIdProfile : Profile
{
	public GetLogByIdProfile()
	{
		CreateMap<Log, GetLogByIdDto>();
	}
}
