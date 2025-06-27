using AutoMapper;
using FluentAssertions;
using FluentValidation;
using Moq;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Users.RegisterUser;
using ReportHub.Domain;

namespace ReportHub.UnitTests.Users;

public class RegisterUserCommandHandlerTests
{
	private Mock<IMapper> mapperMock;
	private Mock<IIdentityService> identityServiceMock;
	private Mock<IValidator<RegisterUserCommand>> validatorMock;

	[SetUp]
	public void Setup()
	{
		mapperMock = new Mock<IMapper>();
		identityServiceMock = new Mock<IIdentityService>();
		validatorMock = new Mock<IValidator<RegisterUserCommand>>();
	}

	[Test]
	public async Task Handle_WhenEmailIsUnique_ShouldReturnUserId()
	{
		// Arrange
		var email = "email@gmail.com";
		var password = "password1234";
		var firstName = "FirstName";
		var lastName = "LastName";
		var user = new User
		{
			Id = Guid.NewGuid(),
			Email = email,
			LastName = lastName,
			FirstName = firstName,
		};

		var command = new RegisterUserCommand(firstName, lastName, email, password);

		mapperMock.Setup(mapper => mapper.Map<User>(command)).Returns(user);

		identityServiceMock.Setup(service => service.RegisterAsync(user, password));

		var handler = new RegisterUserCommandHandler(
			mapperMock.Object, identityServiceMock.Object, validatorMock.Object);

		// Act
		var result = await handler.Handle(command, default);

		// Assert
		result.Should().NotBeEmpty();
		result.Should().Be(user.Id);

		mapperMock.Verify(mapper => mapper.Map<User>(It.IsAny<RegisterUserCommand>()), Times.Once);

		identityServiceMock.Verify(identity => identity.RegisterAsync(It.IsAny<User>(), It.IsAny<string>()), Times.Once);
	}
}
