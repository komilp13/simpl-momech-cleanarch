using Microsoft.Extensions.Logging;
using Moq;
using Simpl.Mobile.Mechanic.Core.Exceptions;

namespace ApplicationTests.Features;

public class RegisterUserTests
{
  private static IEnumerable<TestDataWrapper<Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Request, string[]>> ValidationTestCases()
  {
    // Missing first name and email
    yield return new TestDataWrapper<Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Request, string[]>
    {
      Value = new Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Request(
        firstName: "",
        lastName: "Stamos",
        email: "",
        password: "password123",
        phone: "1234567890"
      ),
      Expected = ["First Name is required", "Email is required"]
    };
    
    // Missing last name and email
    yield return new TestDataWrapper<Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Request, string[]>
    {
      Value = new Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Request(
        firstName: "Henry",
        lastName: "",
        email: "",
        password: "password123",
        phone: "1234567890"
      ),
      Expected = ["Last Name is required", "Email is required"]
    };
    
    // Missing last name, invalid email, and invalid phone
    yield return new TestDataWrapper<Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Request, string[]>
    {
      Value = new Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Request(
        firstName: "Henry",
        lastName: "a",
        email: "justemail",
        password: "password123",
        phone: "123"
      ),
      Expected = ["Last Name must be at least 4 characters long", "Invalid email format", "Phone must be 10 characters in length"]
    };
    
    // Missing last name, invalid email, and invalid phone (with alpha characters)
    yield return new TestDataWrapper<Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Request, string[]>
    {
      Value = new Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Request(
        firstName: "Henry",
        lastName: "a",
        email: "justemail",
        password: "password123",
        phone: "123aa"
      ),
      Expected = ["Last Name must be at least 4 characters long", "Invalid email format", "Phone must be 10 characters in length", "Phone must contain only digits"]
    };
  }

  private Mock<ILogger<Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Handler>> _loggerMock;

  [SetUp]
  public void Setup()
  {
    _loggerMock = new Mock<ILogger<Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Handler>>();
  }

  #region Simple Validation Scenarios

  [TestCaseSource(nameof(ValidationTestCases))]
  public async Task RegisterUser_WithMissingData_ReturnsErrors(TestDataWrapper<Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Request, string[]> data)
  {
    // arrange
    var userRepoMock = new Mock<Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories.IUserRepository>();
    userRepoMock
      .Setup(repo => repo.DoesEmailExistAsync(It.IsAny<string>()))
      .ReturnsAsync(false);
    userRepoMock
      .Setup(repo => repo.InsertUserAsync(It.IsAny<Simpl.Mobile.Mechanic.Core.Domain.Customer>()))
      .ReturnsAsync(true);

    var handler = new Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Handler(_loggerMock.Object, userRepoMock.Object);

    // act
    var response = await handler.Handle(data.Value);

    // assert
    Assert.That(response.Data, Is.False);
    foreach (var expectedError in data.Expected)
    {
      Assert.That(response.Errors, Does.Contain(expectedError));
    }
  }

  #endregion

  [Test]
  public async Task RegisterUser_WithAllValidData_IsSuccessful()
  {
    // arrange
    var userRepoMock = new Mock<Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories.IUserRepository>();
    userRepoMock
      .Setup(repo => repo.DoesEmailExistAsync(It.IsAny<string>()))
      .ReturnsAsync(false);
    userRepoMock
      .Setup(repo => repo.InsertUserAsync(It.IsAny<Simpl.Mobile.Mechanic.Core.Domain.Customer>()))
      .ReturnsAsync(true);

    var request = new Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Request(
      firstName: "John",
      lastName: "Stamos",
      email: "test@example.com",
      password: "Password123",
      phone: "1234567890"
    );
    var handler = new Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Handler(_loggerMock.Object, userRepoMock.Object);

    // act
    var response = await handler.Handle(request);

    // assert
    Assert.That(response.Data, Is.True);
    Assert.That(response.Errors, Is.Empty);
  }
  
  [Test]
  public async Task RegisterUser_WithExistingEmail_ReturnsError()
  {
    // arrange
    var userRepoMock = new Mock<Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories.IUserRepository>();
    userRepoMock
      .Setup(repo => repo.DoesEmailExistAsync(It.IsAny<string>()))
      .ReturnsAsync(true);
    userRepoMock
      .Setup(repo => repo.InsertUserAsync(It.IsAny<Simpl.Mobile.Mechanic.Core.Domain.Customer>()))
      .ReturnsAsync(true);

    var request = new Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Request(
      firstName: "John",
      lastName: "Stamos",
      email: "test@example.com",
      password: "Password123",
      phone: "1234567890"
    );
    var handler = new Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Handler(_loggerMock.Object, userRepoMock.Object);

    // act
    var response = await handler.Handle(request);

    // assert
    Assert.That(response.Data, Is.False);
    Assert.That(response.Errors, Contains.Item("Email already exists"));
  }
  
  [Test]
  public async Task RegisterUser_AllValidData_ButUserCreationFails_ReturnsError()
  {
    // arrange
    var request = new Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Request(
      firstName: "John",
      lastName: "Stamos",
      email: "test@example.com",
      password: "Password123",
      phone: "1234567890"
    );

    var userRepoMock = new Mock<Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories.IUserRepository>();
    userRepoMock
      .Setup(repo => repo.DoesEmailExistAsync(It.IsAny<string>()))
      .ReturnsAsync(false);
    userRepoMock
      .Setup(repo => repo.InsertUserAsync(It.IsAny<Simpl.Mobile.Mechanic.Core.Domain.Customer>()))
      .ThrowsAsync(new DatabaseException($"Failed to create user account for email: {request.email}"));

    var handler = new Simpl.Mobile.Mechanic.Application.Features.RegisterUser.Handler(_loggerMock.Object, userRepoMock.Object);

    // act
    var response = await handler.Handle(request);

    // assert
    Assert.That(response.Data, Is.False);
    Assert.That(response.Errors, Contains.Item($"Failed to create user account for email: {request.email}"));
  }

}
