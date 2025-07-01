using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Organizations.GetMyOrganizationsList;

public class GetMyOrganizationsListQuery : IQuery<IEnumerable<GetMyOrganizationsListDto>>
{
}

[RequiresUserRole(UserRoles.Admin, UserRoles.User)]
public class GetMyOrganizationsListQueryHandler(
	IMapper mapper,
	ICurrentUserService service,
	IOrganizationMemberRepository repository)
	: IQueryHandler<GetMyOrganizationsListQuery, IEnumerable<GetMyOrganizationsListDto>>
{
	public async Task<IEnumerable<GetMyOrganizationsListDto>> Handle(GetMyOrganizationsListQuery request, CancellationToken cancellationToken)
	{
		var organizations = await repository
			.SelectAll()
			.IgnoreQueryFilters()
			.Where(t => t.UserId == service.UserId && !t.IsDeleted)
			.Select(t => t.Organization)
			.ProjectTo<GetMyOrganizationsListDto>(mapper.ConfigurationProvider)
			.ToListAsync(cancellationToken);

		return organizations;
	}
}
