using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Invoices.GetInvoicesList;

public class GetInvoicesListQuery(Guid organizationId) : IQuery<IEnumerable<GetInvoicesListDto>>, IOrganizationRequest
{
	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class GetInvoicesListQueryHandler(IMapper mapper, IInvoiceRepository repository)
	: IQueryHandler<GetInvoicesListQuery, IEnumerable<GetInvoicesListDto>>
{
	public async Task<IEnumerable<GetInvoicesListDto>> Handle(GetInvoicesListQuery request, CancellationToken cancellationToken)
	{
		var invoices = await repository
			.SelectAll()
			.ProjectTo<GetInvoicesListDto>(mapper.ConfigurationProvider)
			.ToListAsync(cancellationToken);

		return invoices;
	}
}
