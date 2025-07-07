using Microsoft.EntityFrameworkCore;
using Quartz;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;

namespace ReportHub.Infrastructure.Quartz;

public class ReportEmailSenderJob(
	IEmailService emailService,
	IAsposeService asposeService,
	IPlanRepository planRepository,
	IItemRepository itemRepository,
	IInvoiceRepository invoiceRepository) : IJob
{
	public const string Name = nameof(ReportEmailSenderJob);

	public async Task Execute(IJobExecutionContext context)
	{
		var data = context.MergedJobDataMap;
		var email = data.GetString("email");
		var organizationId = data.GetGuid("organizationId");

		var plans = planRepository.SelectAll().IgnoreQueryFilters().Where(t => t.OrganizationId == organizationId && !t.IsDeleted);
		var items = itemRepository.SelectAll().IgnoreQueryFilters().Where(t => t.OrganizationId == organizationId && !t.IsDeleted);
		var invoices = invoiceRepository.SelectAll().IgnoreQueryFilters().Where(t => t.OrganizationId == organizationId && !t.IsDeleted);

		var reportBytes = await asposeService.GenerateAsync(invoices, items, plans);

		await emailService.SendAsync(email, "Email", @$"King in the north", reportBytes);
	}
}
