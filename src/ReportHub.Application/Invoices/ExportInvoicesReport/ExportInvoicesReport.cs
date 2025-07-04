using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Invoices.ExportInvoicesReport;

public class ExportInvoicesReportQuery(ExportInvoicesReportFilter filter, Guid organizationId)
	: IQuery<ExportInvoicesReportDto>, IOrganizationRequest
{
	public ExportInvoicesReportFilter Filter { get; set; } = filter;

	public Guid OrganizationId { get; set; } = organizationId;
}
