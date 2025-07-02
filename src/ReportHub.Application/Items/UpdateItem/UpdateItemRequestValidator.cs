using FluentValidation;

namespace ReportHub.Application.Items.UpdateItem;

public class UpdateItemRequestValidator : AbstractValidator<UpdateItemRequest>
{
	public UpdateItemRequestValidator()
	{
		RuleFor(request => request.Name).MaximumLength(200).NotEmpty();
		RuleFor(request => request.Description).MaximumLength(500);
		RuleFor(request => request.Price).GreaterThan(0);
		RuleFor(request => request.CurrencyCode).MaximumLength(3).NotEmpty();
	}
}
