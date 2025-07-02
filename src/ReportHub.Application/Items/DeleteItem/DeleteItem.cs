using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Items.DeleteItem;

public class DeleteItemCommand(Guid id, Guid organizationId) : ICommand<bool>, IOrganizationRequest
{
	public Guid Id { get; set; } = id;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin)]
public class DeleteItemCommandHandler(IItemRepository repository) : ICommandHandler<DeleteItemCommand, bool>
{
	public async Task<bool> Handle(DeleteItemCommand request, CancellationToken cancellationToken)
	{
		var item = await repository.SelectAsync(t => t.Id == request.Id)
			?? throw new NotFoundException($"Item is not found with this id: {request.Id}");

		var result = await repository.DeleteAsync(item);

		return result;
	}
}
