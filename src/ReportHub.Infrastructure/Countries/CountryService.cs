using Newtonsoft.Json.Linq;
using ReportHub.Application.Common.Interfaces.Services;

namespace ReportHub.Infrastructure.Countries;

public class CountryService(HttpClient client) : ICountryService
{
	public async Task<string> GetCurrencyCodeByCountryCodeAsync(string alphaCode)
	{
		var response = await client.GetStringAsync(alphaCode);
		var json = JObject.Parse(response);

		return json["currencies"]?[0]?["code"].ToString();
	}

	public async Task<bool> VerifyByAlphaCodeAsync(string alphaCode)
	{
		var response = await client.GetAsync(alphaCode);

		return response.IsSuccessStatusCode;
	}
}
