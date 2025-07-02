using Newtonsoft.Json.Linq;
using ReportHub.Application.Common.Interfaces.Services;

namespace ReportHub.Infrastructure.Currencies;

public class CurrencyService(HttpClient client) : ICurrencyService
{
	public async Task<decimal> ExchangeAsync(string from, string to, decimal amount)
	{
		var response = await client.GetStringAsync($"pair/{from}/{to}/{amount}");
		var json = JObject.Parse(response);

		return json.Value<decimal>("conversion_result");
	}

	public async Task<bool> VerifyByAlphaCodeAsync(string alphaCode)
	{
		var response = await client.GetAsync($"latest/{alphaCode}");

		return response.IsSuccessStatusCode;
	}
}
