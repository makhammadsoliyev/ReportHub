using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Invoices.GetInvoicesRevenueCalculation;

public class GetInvoicesRevenueCalculationQuery(GetInvoicesRevenueCalculationFilter filter, Guid organizationId)
	: IQuery<GetInvoicesRevenueCalculationDto>, IOrganizationRequest
{
	public GetInvoicesRevenueCalculationFilter Filter { get; set; } = filter;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class GetInvoicesRevenueCalculationQueryHandler(
	ICountryService countryService,
	ICurrencyService currencyService,
	IInvoiceRepository invoiceRepository,
	ICustomerRepository customerRepository,
	IOrganizationRepository organizationRepository)
	: IQueryHandler<GetInvoicesRevenueCalculationQuery, GetInvoicesRevenueCalculationDto>
{
	public async Task<GetInvoicesRevenueCalculationDto> Handle(GetInvoicesRevenueCalculationQuery request, CancellationToken cancellationToken)
	{
		var organization = await organizationRepository.SelectAsync(t => t.Id == request.OrganizationId);
		var currencyCode = await countryService.GetCurrencyCodeByCountryCodeAsync(organization.CountryCode);
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

		var exchangedAmounts = await Task.WhenAll(
			invoices.Select(invoice => currencyService.ExchangeAsync(
				invoice.CurrencyCode, currencyCode, invoice.Price, DateOnly.FromDateTime(invoice.IssueDate))));

		return new GetInvoicesRevenueCalculationDto
		{
			CurrencyCode = currencyCode,
			Amount = exchangedAmounts.Sum(),
			EndDate = request.Filter.EndDate,
			StartDate = request.Filter.StartDate,
			CustomerId = request.Filter.CustomerId,
		};
	}
}
