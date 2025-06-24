using Microsoft.AspNetCore.Identity;
using ReportHub.Domain.Common;

namespace ReportHub.Domain;

public class User : IdentityUser<Guid>, IEntity
{
	public string FirstName { get; set; }

	public string LastName { get; set; }
}
