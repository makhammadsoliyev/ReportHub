using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Organizations.GetOrganizationRolesList;

public class GetOrganizationRolesListQuery : IQuery<IEnumerable<GetOrganizationRolesListDto>>
{
}

public class GetOrganizationRolesListQueryHandler(IMapper mapper, IOrganizationRoleRepository repository)
	: IQueryHandler<GetOrganizationRolesListQuery, IEnumerable<GetOrganizationRolesListDto>>
{
	public async Task<IEnumerable<GetOrganizationRolesListDto>> Handle(GetOrganizationRolesListQuery request, CancellationToken cancellationToken)
	{
		var roles = await repository
			.SelectAll()
			.ProjectTo<GetOrganizationRolesListDto>(mapper.ConfigurationProvider)
			.ToListAsync(cancellationToken);

		return roles;
	}
}
