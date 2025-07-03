using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Plans.UpdatePlan;

public class UpdatePlanProfile : Profile
{
	public UpdatePlanProfile()
	{
		CreateMap<UpdatePlanRequest, Plan>();
	}
}
