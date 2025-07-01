using AutoMapper;
using ReportHub.Application.Common.Attributes;
using ReportHub.Application.Common.Constants;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Messaging;
using ReportHub.Domain;

namespace ReportHub.Application.Organizations.AddOrganizationMember;

public class AddOrganizationMemberCommand(AddOrganizationMemberRequest organizationMember, Guid organizationId)
	: ICommand<Guid>, IOrganizationRequest
{
	public AddOrganizationMemberRequest OrganizationMember { get; set; } = organizationMember;

	public Guid OrganizationId { get; set; } = organizationId;
}

[RequiresOrganizationRole(OrganizationRoles.Owner)]
public class AddOrganizationMemberHandler(
	IMapper mapper,
	IUserRepository userRepository,
	IOrganizationRoleRepository organizationRoleRepository,
	IOrganizationMemberRepository organizationMemberRepository)
	: ICommandHandler<AddOrganizationMemberCommand, Guid>
{
	public async Task<Guid> Handle(AddOrganizationMemberCommand request, CancellationToken cancellationToken)
	{
		var isUserFound = await userRepository.AnyAsync(user => user.Id == request.OrganizationMember.UserId);
		if (!isUserFound)
		{
			throw new NotFoundException($"User is not found with this id: {request.OrganizationMember.UserId}");
		}

		var isRoleFound = await organizationRoleRepository.AnyAsync(role => role.Id == request.OrganizationMember.RoleId);
		if (!isRoleFound)
		{
			throw new NotFoundException($"Role is not found with this id: {request.OrganizationMember.RoleId}");
		}

		var organizationMember = mapper.Map<OrganizationMember>(request.OrganizationMember);
		organizationMember.OrganizationId = request.OrganizationId;

		await organizationMemberRepository.InsertAsync(organizationMember);

		return organizationMember.Id;
	}
}
