using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Users.GetUserList;

public class GetUserListQuery : IQuery<IEnumerable<GetUserListDto>>
{
}

[RequiresUserRole(UserRoles.Admin)]
public class GetUserListQueryHandler(IMapper mapper, IUserRepository repository) : IRequestHandler<GetUserListQuery, IEnumerable<GetUserListDto>>
{
	public async Task<IEnumerable<GetUserListDto>> Handle(GetUserListQuery request, CancellationToken cancellationToken)
	{
		var users = await repository
			.SelectAll()
			.ProjectTo<GetUserListDto>(mapper.ConfigurationProvider)
			.ToListAsync(cancellationToken);

		return users;
	}
}
