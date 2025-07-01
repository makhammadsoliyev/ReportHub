using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Organizations.GetMyOrganizationsList;

public class GetMyOrganizationsListProfile : Profile
{
	public GetMyOrganizationsListProfile()
	{
		CreateMap<Organization, GetMyOrganizationsListDto>();
	}
}
