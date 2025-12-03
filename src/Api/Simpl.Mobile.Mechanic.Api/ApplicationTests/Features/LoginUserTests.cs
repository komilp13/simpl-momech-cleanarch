using Microsoft.Extensions.Logging;
using Moq;

namespace ApplicationTests.Features;

public class LoginUserTests
{
  private Mock<ILogger<Simpl.Mobile.Mechanic.Application.Features.LoginUser.Handler>> _loggerMock;

  [SetUp]
  public void Setup()
  {
    _loggerMock = new Mock<ILogger<Simpl.Mobile.Mechanic.Application.Features.LoginUser.Handler>>();
  }

  [Test]
  public async Task LoginUser_WithValidEmailPasswordCombo_ReturnsToken()
  {
    // arrange
    var userRepoMock = new Mock<Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories.IUserRepository>();
    userRepoMock
      .Setup(repo => repo.IsEmailPasswordValidAsync(It.IsAny<string>(), It.IsAny<string>()))
      .ReturnsAsync(true);

    var request = new Simpl.Mobile.Mechanic.Application.Features.LoginUser.Request(
      email: "test@example.com",
      password: "Password123"
    );
    var handler = new Simpl.Mobile.Mechanic.Application.Features.LoginUser.Handler(_loggerMock.Object, userRepoMock.Object);

    // act
    var response = await handler.Handle(request);

    // assert
    Assert.That(response.Data, Is.Not.Null);
    Assert.That(response.Data!.Token, Is.Not.Empty);
    Assert.That(response.Errors, Is.Empty);
  }

  [Test]
  public async Task LoginUser_WithInvalidEmailPasswordCombo_ReturnsErrors()
  {
    // arrange
    var userRepoMock = new Mock<Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories.IUserRepository>();
    userRepoMock
      .Setup(repo => repo.IsEmailPasswordValidAsync(It.IsAny<string>(), It.IsAny<string>()))
      .ReturnsAsync(false);

    var request = new Simpl.Mobile.Mechanic.Application.Features.LoginUser.Request(
      email: "test@example.com",
      password: "Password123"
    );
    var handler = new Simpl.Mobile.Mechanic.Application.Features.LoginUser.Handler(_loggerMock.Object, userRepoMock.Object);

    // act
    var response = await handler.Handle(request);

    // assert
    Assert.That(response.Data, Is.Null);
    Assert.That(response.Errors, Is.Not.Empty);
    Assert.That(response.Errors, Contains.Item("Invalid email or password"));
  }

  [Test]
  public async Task LoginUser_WithInvalidEmail_ReturnsErrors()
  {
    // arrange
    var userRepoMock = new Mock<Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories.IUserRepository>();
    userRepoMock
      .Setup(repo => repo.IsEmailPasswordValidAsync(It.IsAny<string>(), It.IsAny<string>()))
      .ReturnsAsync(false);

    var request = new Simpl.Mobile.Mechanic.Application.Features.LoginUser.Request(
      email: "tesexample.com",
      password: "Password123"
    );
    var handler = new Simpl.Mobile.Mechanic.Application.Features.LoginUser.Handler(_loggerMock.Object, userRepoMock.Object);

    // act
    var response = await handler.Handle(request);

    // assert
    Assert.That(response.Data, Is.Null);
    Assert.That(response.Errors, Is.Not.Empty);
    Assert.That(response.Errors, Contains.Item("Invalid email format"));
  }
  
  [Test]
  public async Task LoginUser_WithEmptyEmail_ReturnsErrors()
  {
    // arrange
    var userRepoMock = new Mock<Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories.IUserRepository>();
    userRepoMock
      .Setup(repo => repo.IsEmailPasswordValidAsync(It.IsAny<string>(), It.IsAny<string>()))
      .ReturnsAsync(false);

    var request = new Simpl.Mobile.Mechanic.Application.Features.LoginUser.Request(
      email: "",
      password: "Password123"
    );
    var handler = new Simpl.Mobile.Mechanic.Application.Features.LoginUser.Handler(_loggerMock.Object, userRepoMock.Object);

    // act
    var response = await handler.Handle(request);

    // assert
    Assert.That(response.Data, Is.Null);
    Assert.That(response.Errors, Is.Not.Empty);
    Assert.That(response.Errors, Contains.Item("Email is required"));
  }
  
  [Test]
  public async Task LoginUser_WithEmptyPassword_ReturnsErrors()
  {
    // arrange
    var userRepoMock = new Mock<Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories.IUserRepository>();
    userRepoMock
      .Setup(repo => repo.IsEmailPasswordValidAsync(It.IsAny<string>(), It.IsAny<string>()))
      .ReturnsAsync(false);

    var request = new Simpl.Mobile.Mechanic.Application.Features.LoginUser.Request(
      email: "test@example.com",
      password: ""
    );
    var handler = new Simpl.Mobile.Mechanic.Application.Features.LoginUser.Handler(_loggerMock.Object, userRepoMock.Object);

    // act
    var response = await handler.Handle(request);

    // assert
    Assert.That(response.Data, Is.Null);
    Assert.That(response.Errors, Is.Not.Empty);
    Assert.That(response.Errors, Contains.Item("Password is required"));
  }
  
  [Test]
  public async Task LoginUser_WithPasswordLengthLessThan8Chars_ReturnsErrors()
  {
    // arrange
    var userRepoMock = new Mock<Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories.IUserRepository>();
    userRepoMock
      .Setup(repo => repo.IsEmailPasswordValidAsync(It.IsAny<string>(), It.IsAny<string>()))
      .ReturnsAsync(false);

    var request = new Simpl.Mobile.Mechanic.Application.Features.LoginUser.Request(
      email: "test@example.com",
      password: ""
    );
    var handler = new Simpl.Mobile.Mechanic.Application.Features.LoginUser.Handler(_loggerMock.Object, userRepoMock.Object);

    // act
    var response = await handler.Handle(request);

    // assert
    Assert.That(response.Data, Is.Null);
    Assert.That(response.Errors, Is.Not.Empty);
    Assert.That(response.Errors, Contains.Item("Password must be at least 8 characters long"));
  }
}
