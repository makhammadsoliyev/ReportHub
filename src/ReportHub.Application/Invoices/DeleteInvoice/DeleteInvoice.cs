using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Invoices.DeleteInvoice;

public class DeleteInvoiceCommand(Guid id, Guid organizationId) : ICommand<bool>, IOrganizationRequest
{
	public Guid Id { get; set; } = id;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner)]
public class DeleteInvoiceCommandHandler(IInvoiceRepository repository) : ICommandHandler<DeleteInvoiceCommand, bool>
{
	public async Task<bool> Handle(DeleteInvoiceCommand request, CancellationToken cancellationToken)
	{
		var invoice = await repository.SelectAsync(t => t.Id == request.Id)
			?? throw new NotFoundException($"Invoice is not found with this id: {request.Id}");

		var result = await repository.DeleteAsync(invoice);

		return result;
	}
}
