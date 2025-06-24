using AutoMapper;
using FluentValidation;
using ReportHub.Application.Common.Interfaces;
using ReportHub.Application.Common.Messaging;
using ReportHub.Domain;

namespace ReportHub.Application.Users.RegisterUser;

public class RegisterUserCommand : ICommand<Guid>
{
	public string FirstName { get; set; }

	public string LastName { get; set; }

	public string Email { get; set; }

	public string Password { get; set; }
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
