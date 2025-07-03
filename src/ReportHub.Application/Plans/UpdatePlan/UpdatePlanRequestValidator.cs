using FluentValidation;

namespace ReportHub.Application.Plans.UpdatePlan;

public class UpdatePlanRequestValidator : AbstractValidator<UpdatePlanRequest>
{
	public UpdatePlanRequestValidator()
	{
		RuleFor(request => request.Title).NotEmpty().MaximumLength(200);
		RuleFor(request => request.StartDate).LessThanOrEqualTo(request => request.EndDate);
	}
}
