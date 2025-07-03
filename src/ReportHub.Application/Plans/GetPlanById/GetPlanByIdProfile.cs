using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Plans.GetPlanById;

public class GetPlanByIdProfile : Profile
{
	public GetPlanByIdProfile()
	{
		CreateMap<Plan, GetPlanByIdDto>();
	}
}
