using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Plans.CreatePlan;

public class CreatePlanProfile : Profile
{
	public CreatePlanProfile()
	{
		CreateMap<CreatePlanRequest, Plan>();
	}
}
