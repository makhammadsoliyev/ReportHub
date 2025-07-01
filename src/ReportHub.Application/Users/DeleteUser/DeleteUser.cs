using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Users.DeleteUser;

public class DeleteUserCommand(Guid id) : ICommand<bool>
{
	public Guid Id { get; set; } = id;
}

[RequiresUserRole(UserRoles.Admin)]
public class DeleteUserCommandHandler(IUserRepository repository)
	: ICommandHandler<DeleteUserCommand, bool>
{
	public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
	{
		var user = await repository.Select(t => t.Id == request.Id)
			?? throw new NotFoundException($"User is not found with this id: {request.Id}");

		var result = await repository.DeleteAsync(user);

		return result;
	}
}
