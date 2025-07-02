using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Items.GetItemsList;

public class GetItemsListQuery(Guid organizationId) : IQuery<IEnumerable<GetItemsListDto>>, IOrganizationRequest
{
	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class GetItemsListQueryHandler(IMapper mapper, IItemRepository repository)
	: IQueryHandler<GetItemsListQuery, IEnumerable<GetItemsListDto>>
{
	public async Task<IEnumerable<GetItemsListDto>> Handle(GetItemsListQuery request, CancellationToken cancellationToken)
	{
		var items = await repository
			.SelectAll()
			.ProjectTo<GetItemsListDto>(mapper.ConfigurationProvider)
			.ToListAsync(cancellationToken);

		return items;
	}
}
