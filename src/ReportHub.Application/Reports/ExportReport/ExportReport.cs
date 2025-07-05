using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Reports.ExportReport;

public class ExportReportQuery(Guid organizationId) : IQuery<ExportReportDto>, IOrganizationRequest
{
	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin, OrganizationRoles.Operator)]
public class ExportReportQueryHandler(
	IAsposeService service,
	IItemRepository itemRepository,
	IPlanRepository planRepository,
	IInvoiceRepository invoiceRepository)
	: IQueryHandler<ExportReportQuery, ExportReportDto>
{
	public async Task<ExportReportDto> Handle(ExportReportQuery request, CancellationToken cancellationToken)
	{
		var items = itemRepository.SelectAll();
		var plans = planRepository.SelectAll();
		var invoices = invoiceRepository.SelectAll();

		var result = await service.GenerateAsync(invoices, items, plans);

		return new ExportReportDto
		{
			Content = result,
			ContentType = "application/vnd.ms-excel",
			FileName = "report.xls",
		};
	}
}
