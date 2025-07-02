using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Invoices.ExportInvoice;

public class ExportInvoiceQuery(Guid id, Guid organizationId) : IQuery<ExportInvoiceDto>, IOrganizationRequest
{
	public Guid Id { get; set; } = id;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class ExportInvoiceQueryHandler(IPdfService service, IInvoiceRepository repository)
	: IQueryHandler<ExportInvoiceQuery, ExportInvoiceDto>
{
	public async Task<ExportInvoiceDto> Handle(ExportInvoiceQuery request, CancellationToken cancellationToken)
	{
		var invoice = await repository.SelectAsync(t => t.Id == request.Id)
			?? throw new NotFoundException($"Invoice is not found with this id: {request.Id}");

		var content = service.GeneratePdf(invoice);

		return new ExportInvoiceDto
		{
			Content = content,
			ContentType = "application/pdf",
			FileName = $"Invoice_{invoice.InvoiceNumber}",
		};
	}
}
