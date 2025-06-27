using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Organizations.CreateOrganization;

public class CreateOrganizationProfile : Profile
{
	public CreateOrganizationProfile()
	{
		CreateMap<CreateOrganizationCommand, Organization>();
	}
}
