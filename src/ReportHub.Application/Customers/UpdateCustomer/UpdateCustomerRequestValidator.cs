using FluentValidation;

namespace ReportHub.Application.Customers.UpdateCustomer;

public class UpdateCustomerRequestValidator : AbstractValidator<UpdateCustomerRequest>
{
	public UpdateCustomerRequestValidator()
	{
		RuleFor(request => request.Name).NotEmpty().MaximumLength(200);
		RuleFor(request => request.Email).NotEmpty().MaximumLength(200);
		RuleFor(request => request.CountryCode).NotEmpty().MaximumLength(5);
	}
}
