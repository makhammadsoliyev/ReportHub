using System.Text;
using Microsoft.AspNetCore.WebUtilities;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Users.ConfirmUserEmail;

public class ConfirmUserEmailCommand(Guid id, string token) : ICommand<bool>
{
	public Guid Id { get; set; } = id;

	public string Token { get; set; } = token;
}

public class ConfirmUserEmailCommandHandler(IIdentityService service, IUserRepository repository)
	: ICommandHandler<ConfirmUserEmailCommand, bool>
{
	public async Task<bool> Handle(ConfirmUserEmailCommand request, CancellationToken cancellationToken)
	{
		var user = await repository.Select(user => user.Id == request.Id)
			?? throw new NotFoundException($"User is not found with this id: {request.Id}");

		var decodedToken = WebEncoders.Base64UrlDecode(request.Token);
		var token = Encoding.UTF8.GetString(decodedToken);
		var result = await service.ConfirmEmailAsync(user, token);

		return result;
	}
}
