using AutoMapper;
using FluentValidation;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Invoices.UpdateInvoice;

public class UpdateInvoiceCommand(UpdateInvoiceRequest invoice, Guid organizationId)
	: ICommand<Guid>, IOrganizationRequest
{
	public UpdateInvoiceRequest Invoice { get; set; } = invoice;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin)]
public class UpdateInvoiceCommandHandler(
	IMapper mapper,
	ICountryService service,
	IInvoiceRepository invoiceRepository,
	ICustomerRepository customerRepository,
	IValidator<UpdateInvoiceRequest> validator)
	: ICommandHandler<UpdateInvoiceCommand, Guid>
{
	public async Task<Guid> Handle(UpdateInvoiceCommand request, CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(request.Invoice, cancellationToken);

		var invoice = await invoiceRepository.SelectAsync(t => t.Id == request.Invoice.Id)
			?? throw new NotFoundException($"Invoice is not found with this id: {request.Invoice.Id}");

		var customer = await customerRepository.SelectAsync(t => t.Id == request.Invoice.CustomerId)
			?? throw new NotFoundException($"Customer is not found with this id: {request.Invoice.CustomerId}");

		mapper.Map(request.Invoice, invoice);
		invoice.OrganizationId = request.OrganizationId;
		invoice.CurrencyCode = await service.GetCurrencyCodeByCountryCodeAsync(customer.CountryCode);
		await invoiceRepository.UpdateAsync(invoice);

		return invoice.Id;
	}
}
