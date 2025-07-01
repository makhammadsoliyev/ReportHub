using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Organizations.DeleteOrganization;

public class DeleteOrganizationCommand(Guid id) : ICommand<bool>
{
	public Guid Id { get; set; } = id;
}

[RequiresUserRole(UserRoles.Admin)]
public class DeleteOrganizationCommandHandler(IOrganizationRepository repository)
	: ICommandHandler<DeleteOrganizationCommand, bool>
{
	public async Task<bool> Handle(DeleteOrganizationCommand request, CancellationToken cancellationToken)
	{
		var organization = await repository.SelectAsync(t => t.Id == request.Id)
			?? throw new NotFoundException($"Organization is not found with this id: {request.Id}");

		var result = await repository.DeleteAsync(organization);

		return result;
	}
}
