namespace ReportHub.Application.Common.Interfaces.Services;

public interface ICountryService
{
	Task<bool> VerifyByAlphaCodeAsync(string alphaCode);
}
