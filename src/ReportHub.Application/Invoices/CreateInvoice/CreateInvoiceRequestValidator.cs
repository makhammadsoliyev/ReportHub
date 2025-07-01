using FluentValidation;
using ReportHub.Application.Common.Interfaces.Services;

namespace ReportHub.Application.Invoices.CreateInvoice;

public class CreateInvoiceRequestValidator : AbstractValidator<CreateInvoiceRequest>
{
	public CreateInvoiceRequestValidator(IDateTimeService service)
	{
		RuleFor(request => request.IssueDate).LessThanOrEqualTo(service.UtcNow).NotEmpty();
		RuleFor(request => request.DueDate).GreaterThanOrEqualTo(request => request.IssueDate).NotEmpty();
		RuleFor(request => request.PaymentStatus).IsInEnum();
	}
}
