using FluentValidation;

namespace ReportHub.Application.Users.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
	public RegisterUserCommandValidator()
	{
		RuleFor(command => command.FirstName).NotEmpty();
	}
}
