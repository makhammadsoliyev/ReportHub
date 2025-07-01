using ReportHub.Application.Common.Interfaces.Services;

namespace ReportHub.Infrastructure.Currencies;

public class CurrencyService(HttpClient client) : ICurrencyService
{
	public async Task<bool> VerifyByAlphaCodeAsync(string alphaCode)
	{
		var response = await client.GetAsync($"latest/{alphaCode}");

		return response.IsSuccessStatusCode;
	}
}
