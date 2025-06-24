using Microsoft.AspNetCore.Identity;
using ReportHub.Domain.Common;

namespace ReportHub.Domain;

public class Role : IdentityRole<Guid>, IEntity
{
}
