using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Plans.GetPlansList;

public class GetPlansListProfile : Profile
{
	public GetPlansListProfile()
	{
		CreateMap<Plan, GetPlansListDto>();
	}
}
