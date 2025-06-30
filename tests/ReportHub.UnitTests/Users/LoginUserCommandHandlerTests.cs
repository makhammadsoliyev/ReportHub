using FluentAssertions;
using Moq;
using ReportHub.Application.Common.Interfaces.Services;
using ReportHub.Application.Users.LoginUser;
using ReportHub.Domain;

namespace ReportHub.UnitTests.Users;

public class LoginUserCommandHandlerTests
{
	private Mock<ITokenService> tokenServiceMock;
	private Mock<IIdentityService> identityServiceMock;

	[SetUp]
	public void Setup()
	{
		tokenServiceMock = new Mock<ITokenService>();
		identityServiceMock = new Mock<IIdentityService>();
	}

	[Test]
	public async Task Handle_WhenCredentialsValid_ShouldReturnLoginUserDto()
	{
		// Arrange
		var email = "email@gmail.com";
		var password = "password1234";
		var token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50a" +
			"XR5L2NsYWltcy9lbWFpbGFkZHJlc3MiOiJhZG1pbkBleGFkZWwuY29tIiwiaHR0cDovL3NjaGVtYXMueG1sc29hcC5vcmcvd3MvMjAwNS8" +
			"wNS9pZGVudGl0eS9jbGFpbXMvbmFtZWlkZW50aWZpZXIiOiIwMTk3YWM0YS1hMzhhLTc2OTYtYWJiZC1kMWNhZDczYWU3MGEiLCJodHRwO" +
			"i8vc2NoZW1hcy5taWNyb3NvZnQuY29tL3dzLzIwMDgvMDYvaWRlbnRpdHkvY2xhaW1zL3JvbGUiOiJhZG1pbiIsImV4cCI6MTc1MDk2MzY" +
			"wMiwiaXNzIjoiUmVwb3J0SHViIiwiYXVkIjoiUmVwb3J0SHViIn0.GqjhRPvGsnCPlZzwdSSwN-W1_pDczChIEgo72FfwPaw";
		var user = new User { Email = email, EmailConfirmed = true };

		identityServiceMock.Setup(
				service => service.LoginAsync(email, password))
			.ReturnsAsync(user);

		tokenServiceMock.Setup(
				service => service.GenerateAccessTokenAsync(user))
			.ReturnsAsync(token);

		var command = new LoginUserCommand(email, password);
		var handler = new LoginUserCommandHandler(tokenServiceMock.Object, identityServiceMock.Object);

		// Act
		var result = await handler.Handle(command, default);

		// Assert
		result.Should().NotBeNull();
		result.AccessToken.Should().Be(token);

		identityServiceMock.Verify(
			service => service.LoginAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Once);

		tokenServiceMock.Verify(
			service => service.GenerateAccessTokenAsync(It.IsAny<User>()), Times.Once);
	}
}
