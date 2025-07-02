using AutoMapper;
using FluentValidation;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Common.Messaging;

namespace ReportHub.Application.Items.UpdateItem;

public class UpdateItemCommand(UpdateItemRequest item, Guid organizationId) : ICommand<Guid>, IOrganizationRequest
{
	public UpdateItemRequest Item { get; set; } = item;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner, OrganizationRoles.Admin)]
public class UpdateItemCommandHandler(
	IMapper mapper,
	ICurrencyService service,
	IItemRepository repository,
	IValidator<UpdateItemRequest> validator) : ICommandHandler<UpdateItemCommand, Guid>
{
	public async Task<Guid> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
	{
		await validator.ValidateAndThrowAsync(request.Item, cancellationToken);

		var isValidCurrencyCode = await service.VerifyByAlphaCodeAsync(request.Item.CurrencyCode);
		if (!isValidCurrencyCode)
		{
			throw new BadRequestException("Invalid Currency Code");
		}

		var isDuplicateName = await repository.AnyAsync(t =>
			t.Name.ToLower() == request.Item.Name.ToLower() && t.Id != request.Item.Id);
		if (isDuplicateName)
		{
			throw new ConflictException($"Item is already exist with this name: {request.Item.Name}");
		}

		var item = await repository.SelectAsync(t => t.Id == request.Item.Id)
			?? throw new NotFoundException($"Item is not found with this id: {request.Item.Id}");

		mapper.Map(request.Item, item);
		item.OrganizationId = request.OrganizationId;
		await repository.UpdateAsync(item);

		return item.Id;
	}
}
