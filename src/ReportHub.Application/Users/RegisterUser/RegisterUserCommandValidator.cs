using FluentValidation;

namespace ReportHub.Application.Users.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
	public RegisterUserCommandValidator()
	{
		RuleFor(command => command.FirstName).NotEmpty().MaximumLength(200);
		RuleFor(command => command.LastName).NotEmpty().MaximumLength(200);
		RuleFor(command => command.Email).NotEmpty().MaximumLength(200);

		RuleFor(user => user.Password)
			.NotEmpty()
			.MinimumLength(8)
			.Matches("[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
			.Matches("[a-z]").WithMessage("Password must contain at least one lowercase letter.")
			.Matches("[0-9]").WithMessage("Password must contain at least one number.");
	}
}
