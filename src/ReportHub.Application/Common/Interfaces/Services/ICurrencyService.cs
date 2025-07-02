namespace ReportHub.Application.Common.Interfaces.Services;

public interface ICurrencyService
{
	Task<bool> VerifyByAlphaCodeAsync(string alphaCode);

	Task<decimal> ExchangeAsync(string from, string to, decimal amount);
}
