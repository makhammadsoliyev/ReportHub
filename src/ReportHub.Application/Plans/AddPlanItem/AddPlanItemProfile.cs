using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Plans.AddPlanItem;

public class AddPlanItemProfile : Profile
{
	public AddPlanItemProfile()
	{
		CreateMap<AddPlanItemRequest, PlanItem>();
	}
}
