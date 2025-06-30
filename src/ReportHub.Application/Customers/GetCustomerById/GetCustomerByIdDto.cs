namespace ReportHub.Application.Customers.GetCustomerById;

public class GetCustomerByIdDto
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Email { get; set; }

	public string CountryCode { get; set; }
}
