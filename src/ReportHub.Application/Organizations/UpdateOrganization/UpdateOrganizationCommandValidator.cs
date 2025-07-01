using FluentValidation;

namespace ReportHub.Application.Organizations.UpdateOrganization;

public class UpdateOrganizationCommandValidator : AbstractValidator<UpdateOrganizationCommand>
{
	public UpdateOrganizationCommandValidator()
	{
		RuleFor(command => command.Name).NotEmpty().MaximumLength(200);
		RuleFor(command => command.CountryCode).NotEmpty().MaximumLength(5);
	}
}
