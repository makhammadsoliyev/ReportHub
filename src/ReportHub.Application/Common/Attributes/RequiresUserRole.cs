namespace ReportHub.Application.Common.Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class RequiresUserRoleAttribute(params string[] roles) : Attribute
{
	public string[] Roles { get; } = roles;
}