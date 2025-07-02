using AutoMapper;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Invoices.GetInvoiceById;

public class GetInvoiceByIdQuery(Guid id, Guid organizationId) : IQuery<GetInvoiceByIdDto>, IOrganizationRequest
{
	public Guid Id { get; set; } = id;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class GetInvoiceByIdQueryHandler(IMapper mapper, IInvoiceRepository repository)
	: IQueryHandler<GetInvoiceByIdQuery, GetInvoiceByIdDto>
{
	public async Task<GetInvoiceByIdDto> Handle(GetInvoiceByIdQuery request, CancellationToken cancellationToken)
	{
		var invoice = await repository.SelectAsync(t => t.Id == request.Id)
			?? throw new NotFoundException($"Invoice is not found with this id: {request.Id}");

		return mapper.Map<GetInvoiceByIdDto>(invoice);
	}
}
