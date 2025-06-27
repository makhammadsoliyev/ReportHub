using FluentValidation;

namespace ReportHub.Application.Customers.CreateCustomer;

public class CreateCustomerRequestValidator : AbstractValidator<CreateCustomerRequest>
{
	public CreateCustomerRequestValidator()
	{
		RuleFor(request => request.Name).NotEmpty().MaximumLength(200);
		RuleFor(request => request.Email).NotEmpty().MaximumLength(200);
		RuleFor(request => request.CountryCode).NotEmpty().MaximumLength(5);
	}
}
