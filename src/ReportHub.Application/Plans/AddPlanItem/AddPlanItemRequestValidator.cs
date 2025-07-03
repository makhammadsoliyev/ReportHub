using FluentValidation;

namespace ReportHub.Application.Plans.AddPlanItem;

public class AddPlanItemRequestValidator : AbstractValidator<AddPlanItemRequest>
{
	public AddPlanItemRequestValidator()
	{
		RuleFor(request => request.ItemsCount).GreaterThan(0);
	}
}
