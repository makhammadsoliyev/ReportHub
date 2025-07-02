using AutoMapper;
using FluentValidation;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;
using ReportHub.Domain;

namespace ReportHub.Application.Invoices.AddInvoiceItem;

public class AddInvoiceItemCommand(AddInvoiceItemRequest invoiceItem, Guid invoiceId, Guid organizationId)
	: ICommand<Guid>, IOrganizationRequest
{
	public AddInvoiceItemRequest InvoiceItem { get; set; } = invoiceItem;

	public Guid InvoiceId { get; set; } = invoiceId;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin)]
public class AddInvoiceItemCommandHandler(
	IMapper mapper,
	ICurrencyService service,
	IItemRepository itemRepository,
	IInvoiceRepository invoiceRepository,
	IValidator<AddInvoiceItemRequest> validator,
	IInvoiceItemRepository invoiceItemRepository) : ICommandHandler<AddInvoiceItemCommand, Guid>
{
	public async Task<Guid> Handle(AddInvoiceItemCommand request, CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(request.InvoiceItem, cancellationToken);

		var item = await itemRepository.SelectAsync(t => t.Id == request.InvoiceItem.ItemId)
			?? throw new NotFoundException($"Item is not found with this id: {request.InvoiceItem.ItemId}");

		var invoice = await invoiceRepository.SelectAsync(t => t.Id == request.InvoiceId)
			?? throw new NotFoundException($"Invoice is not found with this id: {request.InvoiceId}");

		var invoiceItem = mapper.Map<InvoiceItem>(request.InvoiceItem);
		invoiceItem.InvoiceId = request.InvoiceId;
		invoiceItem.OrganizationId = request.OrganizationId;
		invoiceItem.CurrencyCode = invoice.CurrencyCode;
		invoiceItem.Price = await service.ExchangeAsync(item.CurrencyCode, invoiceItem.CurrencyCode, item.Price * invoiceItem.ItemsCount);

		await invoiceItemRepository.InsertAsync(invoiceItem);

		return invoiceItem.Id;
	}
}
