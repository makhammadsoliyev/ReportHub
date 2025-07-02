using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Invoices.GetInvoiceItemList;

public class GetInvoiceItemListQuery(Guid invoiceId, Guid organizationId)
	: IQuery<IEnumerable<GetInvoiceItemListDto>>, IOrganizationRequest
{
	public Guid InvoiceId { get; set; } = invoiceId;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class GetInvoiceItemListQueryHandler(IMapper mapper, IInvoiceItemRepository repository)
	: IQueryHandler<GetInvoiceItemListQuery, IEnumerable<GetInvoiceItemListDto>>
{
	public async Task<IEnumerable<GetInvoiceItemListDto>> Handle(GetInvoiceItemListQuery request, CancellationToken cancellationToken)
	{
		var invoiceItems = await repository
			.SelectAll()
			.Where(t => t.InvoiceId == request.InvoiceId)
			.ProjectTo<GetInvoiceItemListDto>(mapper.ConfigurationProvider)
			.ToListAsync(cancellationToken);

		return invoiceItems;
	}
}
