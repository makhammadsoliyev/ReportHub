using AutoMapper;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Users.GetUserByEmail;

public class GetUserByEmailQuery(string email) : IQuery<GetUserByEmailDto>
{
	public string Email { get; set; } = email;
}

public class GetUserByEmailQueryHandler(IMapper mapper, IUserRepository repository)
	: IQueryHandler<GetUserByEmailQuery, GetUserByEmailDto>
{
	public async Task<GetUserByEmailDto> Handle(GetUserByEmailQuery request, CancellationToken cancellationToken)
	{
		var user = await repository.Select(t => t.Email == request.Email)
			?? throw new NotFoundException($"User is not found with this email: {request.Email}");

		return mapper.Map<GetUserByEmailDto>(user);
	}
}