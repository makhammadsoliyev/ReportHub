using FluentValidation;

namespace ReportHub.Application.Organizations.CreateOrganization;

public class CreateOrganizationCommandValidator : AbstractValidator<CreateOrganizationCommand>
{
	public CreateOrganizationCommandValidator()
	{
		RuleFor(command => command.Name).NotEmpty().MaximumLength(200);
		RuleFor(command => command.CountryCode).NotEmpty().MaximumLength(5);
	}
}
