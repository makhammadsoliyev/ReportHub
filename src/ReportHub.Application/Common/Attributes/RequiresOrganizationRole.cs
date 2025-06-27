namespace ReportHub.Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RequiresOrganizationRoleAttribute(params string[] roles) : Attribute
{
	public string[] Roles { get; } = roles;
}
