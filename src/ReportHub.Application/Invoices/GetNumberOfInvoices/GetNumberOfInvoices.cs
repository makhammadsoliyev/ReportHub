using Microsoft.EntityFrameworkCore;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Invoices.GetNumberOfInvoices;

public class GetNumberOfInvoicesQuery(GetNumberOfInvoicesFilter filter, Guid organizationId)
	: IQuery<GetNumberOfInvoicesDto>, IOrganizationRequest
{
	public GetNumberOfInvoicesFilter Filter { get; set; } = filter;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class GetNumberOfInvoicesQueryHandler(IInvoiceRepository invoiceRepository, ICustomerRepository customerRepository)
	: IQueryHandler<GetNumberOfInvoicesQuery, GetNumberOfInvoicesDto>
{
	public async Task<GetNumberOfInvoicesDto> Handle(GetNumberOfInvoicesQuery request, CancellationToken cancellationToken)
	{
		var invoices = invoiceRepository.SelectAll();

		if (request.Filter.CustomerId is Guid customerId)
		{
			var isCustomerFound = await customerRepository.AnyAsync(customer => customer.Id == customerId);
			if (!isCustomerFound)
			{
				throw new NotFoundException($"Customer is not found with this id: {request.Filter.CustomerId}");
			}

			invoices = invoices.Where(invoice => invoice.CustomerId == customerId);
		}

		if (request.Filter.StartDate is DateTime startDate)
		{
			invoices = invoices.Where(invoice => startDate <= invoice.IssueDate);
		}

		if (request.Filter.EndDate is DateTime endDate)
		{
			invoices = invoices.Where(invoice => invoice.IssueDate <= endDate);
		}

		var count = await invoices.CountAsync(cancellationToken);

		return new GetNumberOfInvoicesDto
		{
			Count = count,
			StartDate = request.Filter.StartDate,
			EndDate = request.Filter.EndDate,
			CustomerId = request.Filter.CustomerId,
		};
	}
}
