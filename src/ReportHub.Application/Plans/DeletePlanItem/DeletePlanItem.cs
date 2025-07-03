using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Plans.DeletePlanItem;

public class DeletePlanItemCommand(Guid id, Guid planId, Guid organizationId) : ICommand<bool>, IOrganizationRequest
{
	public Guid Id { get; set; } = id;

	public Guid PlanId { get; set; } = planId;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin)]
public class DeletePlanItemCommandHandler(IPlanItemRepository repository) : ICommandHandler<DeletePlanItemCommand, bool>
{
	public async Task<bool> Handle(DeletePlanItemCommand request, CancellationToken cancellationToken)
	{
		var planItem = await repository.SelectAsync(t => t.Id == request.Id)
			?? throw new NotFoundException($"Plan Item is not found with this id: {request.Id}");

		var result = await repository.DeleteAsync(planItem);

		return result;
	}
}
