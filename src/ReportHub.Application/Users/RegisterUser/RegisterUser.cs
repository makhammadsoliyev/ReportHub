using AutoMapper;
using FluentValidation;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;
using ReportHub.Domain;

namespace ReportHub.Application.Users.RegisterUser;

public class RegisterUserCommand(string firstName, string lastName, string email, string password) : ICommand<Guid>
{
	public string FirstName { get; set; } = firstName;

	public string LastName { get; set; } = lastName;

	public string Email { get; set; } = email;

	public string Password { get; set; } = password;
}

public class RegisterUserCommandHandler(
	IMapper mapper,
	IIdentityService identityService,
	IValidator<RegisterUserCommand> validator) : ICommandHandler<RegisterUserCommand, Guid>
{
	public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(request, cancellationToken);

		var user = mapper.Map<User>(request);
		await identityService.RegisterAsync(user, request.Password);

		return user.Id;
	}
}
