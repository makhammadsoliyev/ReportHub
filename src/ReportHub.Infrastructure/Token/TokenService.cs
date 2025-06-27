using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Domain;

namespace ReportHub.Infrastructure.Token;

public class TokenService(
	UserManager<User> userManager,
	IDateTimeService dateTimeService,
	IOptions<JwtOptions> jwtOptions) : ITokenService
{
	private readonly JwtOptions jwtOptions = jwtOptions.Value;

	public async Task<string> GenerateAccessTokenAsync(User user)
	{
		var roles = await userManager.GetRolesAsync(user);
		var claims = new List<Claim>
		{
			new (ClaimTypes.Email, user.Email),
			new (ClaimTypes.NameIdentifier, user.Id.ToString()),
		};

		claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

		var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecurityKey));
		var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
		var expires = dateTimeService.UtcNow.AddMinutes(jwtOptions.ExpiresInMinutes);

		var tokenHandler = new JwtSecurityTokenHandler();
		var token = new JwtSecurityToken(
			issuer: jwtOptions.Issuer,
			audience: jwtOptions.Audience,
			claims: claims,
			expires: expires,
			signingCredentials: signingCredentials);

		return tokenHandler.WriteToken(token);
	}
}
