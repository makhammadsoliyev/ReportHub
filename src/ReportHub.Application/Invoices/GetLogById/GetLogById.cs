using AutoMapper;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Invoices.GetLogById;

public class GetLogByIdQuery(Guid id, Guid organizationId) : IQuery<GetLogByIdDto>, IOrganizationRequest
{
	public Guid Id { get; set; } = id;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class GetLogByIdQueryHandler(IMapper mapper, ILogRepository repository)
	: IQueryHandler<GetLogByIdQuery, GetLogByIdDto>
{
	public async Task<GetLogByIdDto> Handle(GetLogByIdQuery request, CancellationToken cancellationToken)
	{
		var log = await repository.SelectAsync(t => t.Id == request.Id)
			?? throw new NotFoundException($"Log is not found with this id: {request.Id}");

		return mapper.Map<GetLogByIdDto>(log);
	}
}
