using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;
using ReportHub.Domain;

namespace ReportHub.Application.Invoices.ExportInvoice;

public class ExportInvoiceQuery(Guid id, Guid organizationId) : IQuery<ExportInvoiceDto>, IOrganizationRequest
{
	public Guid Id { get; set; } = id;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class ExportInvoiceQueryHandler(
	IPdfService pdfService,
	ILogRepository logRepository,
	ICurrentUserService userService,
	IDateTimeService dateTimeService,
	IInvoiceRepository invoiceRepository)
	: IQueryHandler<ExportInvoiceQuery, ExportInvoiceDto>
{
	public async Task<ExportInvoiceDto> Handle(ExportInvoiceQuery request, CancellationToken cancellationToken)
	{
		var invoice = await invoiceRepository.SelectAsync(t => t.Id == request.Id)
			?? throw new NotFoundException($"Invoice is not found with this id: {request.Id}");
		var log = new Log
		{
			InvoiceId = request.Id,
			TimeStamp = dateTimeService.UtcNow,
			UserId = userService.UserId,
			Status = LogStatus.Success,
		};

		try
		{
			var content = pdfService.GeneratePdf(invoice);
			var result = new ExportInvoiceDto
			{
				Content = content,
				ContentType = "application/pdf",
				FileName = $"Invoice_{invoice.InvoiceNumber}",
			};
			await logRepository.InsertAsync(log);

			return result;
		}
		catch
		{
			log.Status = LogStatus.Failure;
			await logRepository.InsertAsync(log);

			throw;
		}
	}
}
