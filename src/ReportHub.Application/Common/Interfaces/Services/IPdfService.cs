using ReportHub.Domain;

namespace ReportHub.Application.Common.Interfaces.Services;

public interface IPdfService
{
	byte[] GeneratePdf(Invoice invoice);
}
