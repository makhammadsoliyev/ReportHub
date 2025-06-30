using System.Text;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
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
	IEmailService emailService,
	IConfiguration configuration,
	IIdentityService identityService,
	IValidator<RegisterUserCommand> validator) : ICommandHandler<RegisterUserCommand, Guid>
{
	public async Task<Guid> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(request, cancellationToken);
		var user = mapper.Map<User>(request);
		await identityService.RegisterAsync(user, request.Password);

		var confirmEmailToken = await identityService.GenerateEmailConfirmationTokenAsync(user);
		var encodedEmailToken = Encoding.UTF8.GetBytes(confirmEmailToken);
		var validEmailToken = WebEncoders.Base64UrlEncode(encodedEmailToken);
		var baseUrl = configuration["BaseUrl"];

		var url = $"{baseUrl}/users/email-confirmation?id={user.Id}&token={validEmailToken}";

		await emailService.SendAsync(
			user.Email,
			"Email Confirmation!",
			$"<h1>Welcome to Report Hub</h1><p>Please confirm your email by <a href='{url}'>clicking here</a></p>");

		return user.Id;
	}
}
