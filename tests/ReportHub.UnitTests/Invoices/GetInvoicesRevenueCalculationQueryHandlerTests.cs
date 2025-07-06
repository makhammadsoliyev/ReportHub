using System.Linq.Expressions;
using FluentAssertions;
using MockQueryable;
using Moq;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Invoices.GetInvoicesRevenueCalculation;
using ReportHub.Domain;

namespace ReportHub.UnitTests.Invoices;

public class GetInvoicesRevenueCalculationQueryHandlerTests
{
	private Mock<ICountryService> countryServiceMock;
	private Mock<ICurrencyService> currencyServiceMock;
	private Mock<IInvoiceRepository> invoiceRepositoryMock;
	private Mock<ICustomerRepository> customerRepositoryMock;
	private Mock<IOrganizationRepository> organizationRepositoryMock;

	[SetUp]
	public void Setup()
	{
		countryServiceMock = new Mock<ICountryService>();
		currencyServiceMock = new Mock<ICurrencyService>();
		invoiceRepositoryMock = new Mock<IInvoiceRepository>();
		customerRepositoryMock = new Mock<ICustomerRepository>();
		organizationRepositoryMock = new Mock<IOrganizationRepository>();
	}

	[Test]
	public async Task Handle_WhenFilterGiven_ShouldReturnExpectedRevenue()
	{
		// Arrange
		var filter = new GetInvoicesRevenueCalculationFilter
		{
			CustomerId = Guid.NewGuid(),
			StartDate = DateTime.UtcNow.AddDays(-10),
			EndDate = DateTime.UtcNow,
		};
		var organization = new Organization
		{
			CountryCode = "USA",
		};
		var invoices = new List<Invoice>
		{
			new ()
			{
				CustomerId = filter.CustomerId.Value,
				IssueDate = filter.StartDate.Value,
				CurrencyCode = "UZS",
				InvoiceItems =
				[
					new InvoiceItem
					{
						CurrencyCode = "UZS",
						Price = 1000000,
					},
					new InvoiceItem
					{
						CurrencyCode = "UZS",
						Price = 1200000,
					},
				],
			},
			new ()
			{
				CustomerId = Guid.NewGuid(),
				IssueDate = filter.StartDate.Value,
				CurrencyCode = "UZS",
				InvoiceItems =
				[
					new InvoiceItem
					{
						CurrencyCode = "UZS",
						Price = 100000,
					},
					new InvoiceItem
					{
						CurrencyCode = "UZS",
						Price = 1500000,
					},
				],
			},
			new ()
			{
				CustomerId = filter.CustomerId.Value,
				IssueDate = filter.EndDate.Value,
				CurrencyCode = "USD",
				InvoiceItems =
				[
					new InvoiceItem
					{
						CurrencyCode = "USD",
						Price = 100,
					},
					new InvoiceItem
					{
						CurrencyCode = "USD",
						Price = 120,
					},
				],
			},
		}
		.BuildMock();

		var query = new GetInvoicesRevenueCalculationQuery(filter, organization.Id);

		organizationRepositoryMock.Setup(
				repository => repository.SelectAsync(t => t.Id == query.OrganizationId))
			.ReturnsAsync(organization);

		countryServiceMock.Setup(
				service => service.GetCurrencyCodeByCountryCodeAsync("USA"))
			.ReturnsAsync("USD");

		invoiceRepositoryMock.Setup(
				repository => repository.SelectAll())
			.Returns(invoices);

		customerRepositoryMock.Setup(
				repository => repository.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
			.ReturnsAsync(true);

		currencyServiceMock.Setup(
				service => service.ExchangeAsync("UZS", "USD", 2200000, It.IsAny<DateOnly>()))
			.ReturnsAsync(190);

		currencyServiceMock.Setup(
				service => service.ExchangeAsync("USD", "USD", 220, It.IsAny<DateOnly>()))
			.ReturnsAsync(220);

		var handler = new GetInvoicesRevenueCalculationQueryHandler(
			countryServiceMock.Object,
			currencyServiceMock.Object,
			invoiceRepositoryMock.Object,
			customerRepositoryMock.Object,
			organizationRepositoryMock.Object);

		// Act
		var result = await handler.Handle(query, default);

		// Assert
		result.Should().NotBeNull();
		result.StartDate.Should().Be(filter.StartDate.Value);
		result.EndDate.Should().Be(filter.EndDate.Value);
		result.CustomerId.Should().Be(filter.CustomerId.Value);
		result.CurrencyCode.Should().Be("USD");
		result.Amount.Should().Be(410);

		organizationRepositoryMock.Verify(
			repository => repository.SelectAsync(It.IsAny<Expression<Func<Organization, bool>>>()), Times.Once);

		countryServiceMock.Verify(
			service => service.GetCurrencyCodeByCountryCodeAsync(It.IsAny<string>()), Times.Once);

		invoiceRepositoryMock.Verify(
			repository => repository.SelectAll(), Times.Once);

		customerRepositoryMock.Verify(
			repository => repository.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once);

		currencyServiceMock.Verify(
			service => service.ExchangeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateOnly>()), Times.Exactly(2));
	}

	[Test]
	public async Task Handle_WhenCustomerNotFound_ShouldThrowNotFoundException()
	{
		// Arrange
		var filter = new GetInvoicesRevenueCalculationFilter
		{
			CustomerId = Guid.NewGuid(),
		};
		var organization = new Organization();
		var invoices = new List<Invoice>().BuildMock();

		var query = new GetInvoicesRevenueCalculationQuery(filter, organization.Id);

		organizationRepositoryMock.Setup(
				repository => repository.SelectAsync(t => t.Id == query.OrganizationId))
			.ReturnsAsync(organization);

		countryServiceMock.Setup(
				service => service.GetCurrencyCodeByCountryCodeAsync("USA"))
			.ReturnsAsync("USD");

		invoiceRepositoryMock.Setup(
				repository => repository.SelectAll())
			.Returns(invoices);

		customerRepositoryMock.Setup(
				repository => repository.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
			.ReturnsAsync(false);

		var handler = new GetInvoicesRevenueCalculationQueryHandler(
			countryServiceMock.Object,
			currencyServiceMock.Object,
			invoiceRepositoryMock.Object,
			customerRepositoryMock.Object,
			organizationRepositoryMock.Object);

		var result = () => handler.Handle(query, default);

		// Assert
		await result.Should().ThrowAsync<NotFoundException>();

		organizationRepositoryMock.Verify(
			repository => repository.SelectAsync(It.IsAny<Expression<Func<Organization, bool>>>()), Times.Once);

		countryServiceMock.Verify(
			service => service.GetCurrencyCodeByCountryCodeAsync(It.IsAny<string>()), Times.Once);

		invoiceRepositoryMock.Verify(
			repository => repository.SelectAll(), Times.Once);

		customerRepositoryMock.Verify(
			repository => repository.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once);

		currencyServiceMock.Verify(
			service => service.ExchangeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateOnly>()), Times.Never);
	}
}
