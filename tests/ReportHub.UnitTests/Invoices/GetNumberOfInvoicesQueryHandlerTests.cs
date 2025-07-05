using System.Linq.Expressions;
using FluentAssertions;
using MockQueryable;
using Moq;
using ReportHub.Application.Common.Exceptions;
using ReportHub.Application.Common.Interfaces.Repositories;
using ReportHub.Application.Invoices.GetNumberOfInvoices;
using ReportHub.Domain;

namespace ReportHub.UnitTests.Invoices;

public class GetNumberOfInvoicesQueryHandlerTests
{
	private Mock<IInvoiceRepository> invoiceRepositoryMock;

	private Mock<ICustomerRepository> customerRepositoryMock;

	[SetUp]
	public void Setup()
	{
		invoiceRepositoryMock = new Mock<IInvoiceRepository>();
		customerRepositoryMock = new Mock<ICustomerRepository>();
	}

	[Test]
	public async Task Handle_WhenFilterGiven_ReturnsExpectedNumberOfInvoices()
	{
		// Arrange
		var filter = new GetNumberOfInvoicesFilter
		{
			CustomerId = Guid.NewGuid(),
			StartDate = DateTime.UtcNow.AddDays(-10),
			EndDate = DateTime.UtcNow,
		};

		var invoices = new List<Invoice>
		{
			new ()
			{
				CustomerId = filter.CustomerId.Value,
				IssueDate = filter.StartDate.Value,
			},
			new ()
			{
				CustomerId = Guid.NewGuid(),
				IssueDate = filter.StartDate.Value,
			},
			new ()
			{
				CustomerId = filter.CustomerId.Value,
				IssueDate = filter.EndDate.Value,
			},
		}
		.BuildMock();

		customerRepositoryMock.Setup(
				repository => repository.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
			.ReturnsAsync(true);

		invoiceRepositoryMock.Setup(
				repository => repository.SelectAll())
			.Returns(invoices);

		var query = new GetNumberOfInvoicesQuery(filter, Guid.Empty);
		var handler = new GetNumberOfInvoicesQueryHandler(invoiceRepositoryMock.Object, customerRepositoryMock.Object);

		// Act
		var result = await handler.Handle(query, default);

		// Assert
		result.Should().NotBeNull();
		result.Count.Should().Be(2);
		result.StartDate.Should().Be(filter.StartDate.Value);
		result.EndDate.Should().Be(filter.EndDate.Value);

		customerRepositoryMock.Verify(
			repository => repository.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once);

		invoiceRepositoryMock.Verify(
			repository => repository.SelectAll(), Times.Once);
	}

	[Test]
	public async Task Handle_WhenCustomerNotFound_ShouldThrowNotFoundException()
	{
		// Arrange
		var filter = new GetNumberOfInvoicesFilter
		{
			CustomerId = Guid.NewGuid(),
		};
		var invoices = new List<Invoice>().BuildMock();

		customerRepositoryMock.Setup(
				repository => repository.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()))
			.ReturnsAsync(false);

		invoiceRepositoryMock.Setup(
				repository => repository.SelectAll())
			.Returns(invoices);

		var query = new GetNumberOfInvoicesQuery(filter, Guid.Empty);
		var handler = new GetNumberOfInvoicesQueryHandler(invoiceRepositoryMock.Object, customerRepositoryMock.Object);

		// Act
		var result = () => handler.Handle(query, default);

		// Assert
		await result.Should().ThrowAsync<NotFoundException>();

		customerRepositoryMock.Verify(
			repository => repository.AnyAsync(It.IsAny<Expression<Func<Customer, bool>>>()), Times.Once);

		invoiceRepositoryMock.Verify(
			repository => repository.SelectAll(), Times.Once);
	}
}
