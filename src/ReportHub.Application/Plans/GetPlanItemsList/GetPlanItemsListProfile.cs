using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Plans.GetPlanItemsList;

public class GetPlanItemsListProfile : Profile
{
	public GetPlanItemsListProfile()
	{
		CreateMap<PlanItem, GetPlanItemsListDto>();
	}
}
