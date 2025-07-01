using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Organizations.GetOrganizationMembersList;

public class GetOrganizationMembersListProfile : Profile
{
	public GetOrganizationMembersListProfile()
	{
		CreateMap<OrganizationMember, GetOrganizationMembersListDto>()
			.ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.User.Id))
			.ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.User.Email))
			.ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.User.LastName))
			.ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.User.FirstName))
			.ForMember(dest => dest.OrganizationRole, opt => opt.MapFrom(src => src.OrganizationRole.Name));
	}
}
