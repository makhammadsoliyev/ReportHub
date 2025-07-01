using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Organizations.GetOrganizationsList;

public class GetOrganizationsListQuery : IQuery<IEnumerable<GetOrganizationListDto>>
{
}

[RequiresUserRole(UserRoles.Admin)]
public class GetOrganizationsListQueryHandler(IMapper mapper, IOrganizationRepository repository)
	: IQueryHandler<GetOrganizationsListQuery, IEnumerable<GetOrganizationListDto>>
{
	public async Task<IEnumerable<GetOrganizationListDto>> Handle(GetOrganizationsListQuery request, CancellationToken cancellationToken)
	{
		var organizations = await repository
			.SelectAll()
			.ProjectTo<GetOrganizationListDto>(mapper.ConfigurationProvider)
			.ToListAsync(cancellationToken);

		return organizations;
	}
}
