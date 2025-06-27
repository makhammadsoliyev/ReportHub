using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Users.GetUserList;

public class GetUserListProfile : Profile
{
	public GetUserListProfile()
	{
		CreateMap<User, GetUserListDto>();
	}
}
