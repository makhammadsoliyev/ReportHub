using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Organizations.UpdateOrganization;

public class UpdateOrganizationProfile : Profile
{
	public UpdateOrganizationProfile()
	{
		CreateMap<UpdateOrganizationCommand, Organization>();
	}
}
