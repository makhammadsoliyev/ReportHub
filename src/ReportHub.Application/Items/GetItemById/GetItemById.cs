using AutoMapper;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Items.GetItemById;

public class GetItemByIdQuery(Guid id, Guid organizationId) : IQuery<GetItemByIdDto>, IOrganizationRequest
{
	public Guid Id { get; set; } = id;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class GetItemByIdQueryHandler(IMapper mapper, IItemRepository repository)
	: IQueryHandler<GetItemByIdQuery, GetItemByIdDto>
{
	public async Task<GetItemByIdDto> Handle(GetItemByIdQuery request, CancellationToken cancellationToken)
	{
		var item = await repository.SelectAsync(t => t.Id == request.Id)
			?? throw new NotFoundException($"Item is not found with this id: {request.Id}");

		return mapper.Map<GetItemByIdDto>(item);
	}
}
