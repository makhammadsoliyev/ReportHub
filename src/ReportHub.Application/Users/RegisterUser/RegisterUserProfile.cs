using AutoMapper;
using ReportHub.Domain;

namespace ReportHub.Application.Users.RegisterUser;

public class RegisterUserProfile : Profile
{
	public RegisterUserProfile()
	{
		CreateMap<RegisterUserCommand, User>();
	}
}
