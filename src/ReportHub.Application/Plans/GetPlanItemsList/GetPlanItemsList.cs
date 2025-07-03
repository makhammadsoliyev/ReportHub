using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Plans.GetPlanItemsList;

public class GetPlanItemsListQuery(Guid planId, Guid organizationId)
	: IQuery<IEnumerable<GetPlanItemsListDto>>, IOrganizationRequest
{
	public Guid PlanId { get; set; } = planId;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin)]
public class GetPlanItemsListQueryHandler(IMapper mapper, IPlanItemRepository repository)
	: IQueryHandler<GetPlanItemsListQuery, IEnumerable<GetPlanItemsListDto>>
{
	public async Task<IEnumerable<GetPlanItemsListDto>> Handle(GetPlanItemsListQuery request, CancellationToken cancellationToken)
	{
		var planItems = await repository
			.SelectAll()
			.Where(t => t.PlanId == request.PlanId)
			.ProjectTo<GetPlanItemsListDto>(mapper.ConfigurationProvider)
			.ToListAsync(cancellationToken);

		return planItems;
	}
}
