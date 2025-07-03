using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Plans.GetPlansList;

public class GetPlansListQuery(Guid organizationId) : IQuery<IEnumerable<GetPlansListDto>>, IOrganizationRequest
{
	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin)]
public class GetPlansListQueryHandler(IMapper mapper, IPlanRepository repository) : IQueryHandler<GetPlansListQuery, IEnumerable<GetPlansListDto>>
{
	public async Task<IEnumerable<GetPlansListDto>> Handle(GetPlansListQuery request, CancellationToken cancellationToken)
	{
		var plans = await repository
			.SelectAll()
			.ProjectTo<GetPlansListDto>(mapper.ConfigurationProvider)
			.ToListAsync(cancellationToken);

		return plans;
	}
}
