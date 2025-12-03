using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Simpl.Mobile.Mechanic.Core;
using Simpl.Mobile.Mechanic.Core.Exceptions;
using Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories;

namespace Simpl.Mobile.Mechanic.Application.Features.RegisterUser;


public record Request(string firstName, string lastName, string email, string password, string phone) : IRequest<BusinessResponse<bool>>;


public class Handler : IRequestHandler<Request, BusinessResponse<bool>>
{
  private readonly ILogger<Handler> _logger;
  private readonly IUserRepository _userRepo;

  public Handler(ILogger<Handler> logger, IUserRepository userRepo)
  {
    _logger = logger;
    _userRepo = userRepo;
  }

  public async Task<BusinessResponse<bool>> Handle(Request request, CancellationToken ct = default)
  {
    // validate the request
    var validator = new RequestValidator(_userRepo);
    var validationResult = validator.Validate(request);

    if (!validationResult.IsValid)
    {
      var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
      return ErrorResponse.Create<bool>(errors);
    }

    try
    {
      // create the account
      var userToCreate = new Core.Domain.Customer
      {
        FirstName = request.firstName,
        LastName = request.lastName,
        Email = request.email,
        Phone1 = request.phone
      };
      await _userRepo.InsertUserAsync(userToCreate);

      _logger.LogInformation("User account created successfully for email: {Email}", request.email);

      // Send welcome email (omitted for brevity)
    }
    catch (DatabaseException ex)
    {
      _logger.LogError("Failed to create user account for email: {Email}", request.email);
      return ErrorResponse.Create<bool>($"Failed to create user account for email: {request.email}");
    }

    return SuccessResponse.Create(true);
  }
}



public class RequestValidator : AbstractValidator<Request>
{
  private readonly IUserRepository _userRepo;

  public RequestValidator(IUserRepository userRepo)
  {
    _userRepo = userRepo;

    RuleFor(x => x.firstName).NotEmpty().WithMessage("First Name is required");
    RuleFor(x => x.firstName).MinimumLength(4).WithMessage("First Name must be at least 4 characters long");

    RuleFor(x => x.lastName).NotEmpty().WithMessage("Last Name is required");
    RuleFor(x => x.lastName).MinimumLength(4).WithMessage("Last Name must be at least 4 characters long");

    RuleFor(x => x.phone).NotEmpty().WithMessage("Phone is required");
    RuleFor(x => x.phone).Matches("^[0-9]+$").WithMessage("Phone must contain only digits");
    RuleFor(x => x.phone).Length(10).WithMessage("Phone must be 10 characters in length");

    RuleFor(x => x.email).NotEmpty().WithMessage("Email is required");
    RuleFor(x => x.email).EmailAddress().WithMessage("Invalid email format");
    RuleFor(x => x.email).MustAsync(IsUniqueEmail).WithMessage("Email already exists");

    RuleFor(x => x.password).NotEmpty().WithMessage("Password is required");
    RuleFor(x => x.password).MinimumLength(8).WithMessage("Password must be at least 8 characters long");
  }

  private async Task<bool> IsUniqueEmail(string email, CancellationToken ct) => !await _userRepo.DoesEmailExistAsync(email);
}

