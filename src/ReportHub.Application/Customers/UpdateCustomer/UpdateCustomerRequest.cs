namespace ReportHub.Application.Customers.UpdateCustomer;

public class UpdateCustomerRequest
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public string Email { get; set; }

	public string CountryCode { get; set; }
}
