using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Organizations.GetOrganizationsList;

public class GetOrganizationsListProfile : Profile
{
	public GetOrganizationsListProfile()
	{
		CreateMap<Organization, GetOrganizationListDto>();
	}
}
