using FluentValidation;

namespace ReportHub.Application.Plans.CreatePlan;

public class CreatePlanRequestValidator : AbstractValidator<CreatePlanRequest>
{
	public CreatePlanRequestValidator()
	{
		RuleFor(request => request.Title).NotEmpty().MaximumLength(200);
		RuleFor(request => request.StartDate).LessThanOrEqualTo(request => request.EndDate);
	}
}
