using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Users.GetUserByEmail;

public class GetUserByEmailProfile : Profile
{
	public GetUserByEmailProfile()
	{
		CreateMap<User, GetUserByEmailDto>();
	}
}
