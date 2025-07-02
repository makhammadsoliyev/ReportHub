using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Invoices.DeleteInvoiceItem;

public class DeleteInvoiceItemCommand(Guid id, Guid invoiceId, Guid organizationId) : ICommand<bool>, IOrganizationRequest
{
	public Guid Id { get; set; } = id;

	public Guid InvoiceId { get; set; } = invoiceId;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin)]
public class DeleteInvoiceItemCommandHandler(IInvoiceItemRepository repository)
	: ICommandHandler<DeleteInvoiceItemCommand, bool>
{
	public async Task<bool> Handle(DeleteInvoiceItemCommand request, CancellationToken cancellationToken)
	{
		var invoiceItem = await repository.SelectAsync(t => t.Id == request.Id)
			?? throw new DirectoryNotFoundException($"Invoice Item is not found with this id: {request.Id}");

		var result = await repository.DeleteAsync(invoiceItem);

		return result;
	}
}
