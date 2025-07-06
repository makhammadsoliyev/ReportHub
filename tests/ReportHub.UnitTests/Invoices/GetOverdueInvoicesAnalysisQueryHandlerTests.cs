using System.Linq.Expressions;
using FluentAssertions;
using MockQueryable;
using Moq;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Invoices.GetOverdueInvoicesAnalysis;
using ReportHub.Domain;

namespace ReportHub.UnitTests.Invoices;

public class GetOverdueInvoicesAnalysisQueryHandlerTests
{
	private Mock<ICountryService> countryServiceMock;
	private Mock<ICurrencyService> currencyServiceMock;
	private Mock<IDateTimeService> dateTimeServiceMock;
	private Mock<IInvoiceRepository> invoiceRepositoryMock;
	private Mock<IOrganizationRepository> organizationRepositoryMock;

	[SetUp]
	public void Setup()
	{
		countryServiceMock = new Mock<ICountryService>();
		currencyServiceMock = new Mock<ICurrencyService>();
		dateTimeServiceMock = new Mock<IDateTimeService>();
		invoiceRepositoryMock = new Mock<IInvoiceRepository>();
		organizationRepositoryMock = new Mock<IOrganizationRepository>();
	}

	[Test]
	public async Task Handle_WhenFilterGiven_ShouldReturnExpectedOverdueAnalysis()
	{
		// Arrange
		var organization = new Organization
		{
			CountryCode = "USA",
		};
		var time = DateTime.UtcNow;
		var invoices = new List<Invoice>
		{
			new ()
			{
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
				DueDate = time.AddDays(-1),
			},
			new ()
			{
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
				DueDate = time.AddDays(12),
			},
			new ()
			{
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
				DueDate = time.AddDays(-12),
			},
		}
		.BuildMock();

		var query = new GetOverdueInvoicesAnalysisQuery(organization.Id);

		organizationRepositoryMock.Setup(
				repository => repository.SelectAsync(t => t.Id == query.OrganizationId))
			.ReturnsAsync(organization);

		countryServiceMock.Setup(
				service => service.GetCurrencyCodeByCountryCodeAsync("USA"))
			.ReturnsAsync("USD");

		invoiceRepositoryMock.Setup(
				repository => repository.SelectAll())
			.Returns(invoices);

		dateTimeServiceMock.Setup(
				service => service.UtcNow)
			.Returns(time);

		currencyServiceMock.Setup(
				service => service.ExchangeAsync("UZS", "USD", 2200000, It.IsAny<DateOnly>()))
			.ReturnsAsync(190);

		currencyServiceMock.Setup(
				service => service.ExchangeAsync("USD", "USD", 220, It.IsAny<DateOnly>()))
			.ReturnsAsync(220);

		var handler = new GetOverdueInvoicesAnalysisQueryHandler(
			countryServiceMock.Object,
			currencyServiceMock.Object,
			dateTimeServiceMock.Object,
			invoiceRepositoryMock.Object,
			organizationRepositoryMock.Object);

		// Act
		var result = await handler.Handle(query, default);

		// Assert
		result.Should().NotBeNull();
		result.Amount.Should().Be(410);
		result.CurrencyCode.Should().Be("USD");
		result.Count.Should().Be(2);

		organizationRepositoryMock.Verify(
			repository => repository.SelectAsync(It.IsAny<Expression<Func<Organization, bool>>>()), Times.Once);

		countryServiceMock.Verify(
			service => service.GetCurrencyCodeByCountryCodeAsync(It.IsAny<string>()), Times.Once);

		invoiceRepositoryMock.Verify(
			repository => repository.SelectAll(), Times.Once);

		dateTimeServiceMock.Verify(
			service => service.UtcNow, Times.AtLeastOnce);

		currencyServiceMock.Verify(
			service => service.ExchangeAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<decimal>(), It.IsAny<DateOnly>()), Times.Exactly(2));
	}
}
