using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Organizations.GetOrganizationMembersList;

public class GetOrganizationMembersListQuery(Guid organizationId)
	: IQuery<IEnumerable<GetOrganizationMembersListDto>>, IOrganizationRequest
{
	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class GetOrganizationMembersListQueryHandler(IMapper mapper, IOrganizationMemberRepository repository)
	: IQueryHandler<GetOrganizationMembersListQuery, IEnumerable<GetOrganizationMembersListDto>>
{
	public async Task<IEnumerable<GetOrganizationMembersListDto>> Handle(GetOrganizationMembersListQuery request, CancellationToken cancellationToken)
	{
		var members = await repository
			.SelectAll()
			.ProjectTo<GetOrganizationMembersListDto>(mapper.ConfigurationProvider)
			.ToListAsync(cancellationToken);

		return members;
	}
}
