using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;
using ReportHub.Domain;

namespace ReportHub.Application.Invoices.GetOverdueInvoicesAnalysis;

public class GetOverdueInvoicesAnalysisQuery(Guid organizationId) : IQuery<GetOverdueInvoicesAnalysisDto>, IOrganizationRequest
{
	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class GetOverdueInvoicesAnalysisQueryHandler(
	ICountryService countryService,
	ICurrencyService currencyService,
	IDateTimeService dateTimeService,
	IInvoiceRepository invoiceRepository,
	IOrganizationRepository organizationRepository)
	: IQueryHandler<GetOverdueInvoicesAnalysisQuery, GetOverdueInvoicesAnalysisDto>
{
	public async Task<GetOverdueInvoicesAnalysisDto> Handle(GetOverdueInvoicesAnalysisQuery request, CancellationToken cancellationToken)
	{
		var organization = await organizationRepository.SelectAsync(t => t.Id == request.OrganizationId);
		var currencyCode = await countryService.GetCurrencyCodeByCountryCodeAsync(organization.CountryCode);
		var invoices = invoiceRepository.SelectAll().Where(t => t.DueDate < dateTimeService.UtcNow && t.PaymentStatus != PaymentStatus.Paid);

		var exchangedAmounts = await Task.WhenAll(
			invoices.Select(invoice => currencyService.ExchangeAsync(
				invoice.CurrencyCode, currencyCode, invoice.Price, DateOnly.FromDateTime(invoice.IssueDate))));

		return new GetOverdueInvoicesAnalysisDto
		{
			Amount = exchangedAmounts.Sum(),
			CurrencyCode = currencyCode,
			Count = invoices.Count(),
		};
	}
}
