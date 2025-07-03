using AutoMapper;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Plans.GetPlanById;

public class GetPlanByIdQuery(Guid id, Guid organizationId) : IQuery<GetPlanByIdDto>, IOrganizationRequest
{
	public Guid Id { get; set; } = id;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin)]
public class GetPlanByIdQueryHandler(IMapper mapper, IPlanRepository repository)
	: IQueryHandler<GetPlanByIdQuery, GetPlanByIdDto>
{
	public async Task<GetPlanByIdDto> Handle(GetPlanByIdQuery request, CancellationToken cancellationToken)
	{
		var plan = await repository.SelectAsync(t => t.Id == request.Id)
			?? throw new NotFoundException($"Plan is not found with this id: {request.Id}");

		return mapper.Map<GetPlanByIdDto>(plan);
	}
}
