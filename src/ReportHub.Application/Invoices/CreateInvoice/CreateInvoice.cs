using AutoMapper;
using FluentValidation;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;
using ReportHub.Domain;

namespace ReportHub.Application.Invoices.CreateInvoice;

public class CreateInvoiceCommand(CreateInvoiceRequest invoice, Guid organizationId)
	: ICommand<Guid>, IOrganizationRequest
{
	public CreateInvoiceRequest Invoice { get; set; } = invoice;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class CreateInvoiceCommandHandler(
	IMapper mapper,
	ICountryService service,
	IInvoiceRepository invoiceRepository,
	ICustomerRepository customerRepository,
	IValidator<CreateInvoiceRequest> validator)
	: ICommandHandler<CreateInvoiceCommand, Guid>
{
	public async Task<Guid> Handle(CreateInvoiceCommand request, CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(request.Invoice, cancellationToken);

		var customer = await customerRepository.SelectAsync(t => t.Id == request.Invoice.CustomerId)
			?? throw new NotFoundException($"Customer is not found with this id: {request.Invoice.CustomerId}");

		var invoice = mapper.Map<Invoice>(request.Invoice);
		invoice.OrganizationId = request.OrganizationId;
		invoice.CurrencyCode = await service.GetCurrencyCodeByCountryCodeAsync(customer.CountryCode);
		await invoiceRepository.InsertAsync(invoice);

		return invoice.Id;
	}
}
