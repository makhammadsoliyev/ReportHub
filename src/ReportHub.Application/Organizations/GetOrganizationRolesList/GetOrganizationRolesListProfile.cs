using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Organizations.GetOrganizationRolesList;

public class GetOrganizationRolesListProfile : Profile
{
	public GetOrganizationRolesListProfile()
	{
		CreateMap<OrganizationRole, GetOrganizationRolesListDto>();
	}
}
