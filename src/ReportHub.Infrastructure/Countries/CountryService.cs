using ReportHub.Application.Common.Interfaces.Services;

namespace ReportHub.Infrastructure.Countries;

public class CountryService(HttpClient client) : ICountryService
{
	public async Task<bool> VerifyByAlphaCodeAsync(string alphaCode)
	{
		var response = await client.GetAsync(alphaCode);

		return response.IsSuccessStatusCode;
	}
}
