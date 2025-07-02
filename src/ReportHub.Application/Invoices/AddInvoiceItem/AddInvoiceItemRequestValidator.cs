using FluentValidation;

namespace ReportHub.Application.Invoices.AddInvoiceItem;

public class AddInvoiceItemRequestValidator : AbstractValidator<AddInvoiceItemRequest>
{
	public AddInvoiceItemRequestValidator()
	{
		RuleFor(request => request.ItemsCount).GreaterThan(0);
	}
}
