using System.Security.Claims;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Simpl.Mobile.Mechanic.Core;
using Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories;
using Simpl.Mobile.Mechanic.Core.Interfaces.Providers;

namespace Simpl.Mobile.Mechanic.Application.Features.LoginUser;


public record Request(string email, string password) : IRequest<BusinessResponse<Response>>;

public class Response
{
    public string Token { get; set; } = string.Empty;
}


public class Handler : IRequestHandler<Request, BusinessResponse<Response>>
{
    private readonly ILogger<Handler> _logger;
    private readonly IUserRepository _userRepo;
    private readonly IJwtProvider _jwtProvider;

    public Handler(ILogger<Handler> logger, IUserRepository userRepo, IJwtProvider jwtProvider)
    {
        _logger = logger;
        _userRepo = userRepo;
        _jwtProvider = jwtProvider;
    }

    public async Task<BusinessResponse<Response>> Handle(Request request, CancellationToken ct = default)
    {
        // validate the request
        var validator = new RequestValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
            return ErrorResponse.Create<Response>(errors);
        }

        // Check db for the email/password combo
        bool isValid = await _userRepo.IsEmailPasswordValidAsync(request.email, request.password);
        if (!isValid)
        {
            return ErrorResponse.Create<Response>(["Invalid email or password"]);
        }

        // all iz well, generate a token
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Email, request.email)
        };
        var token = await _jwtProvider.GenerateJwtToken(claims.ToArray(), TimeSpan.FromHours(1));
        return new BusinessResponse<Response>(new Response { Token = token }, Array.Empty<string>());
    }
}



public class RequestValidator : AbstractValidator<Request>
{
    public RequestValidator()
    {
        RuleFor(x => x.email).NotEmpty().WithMessage("Email is required");
        RuleFor(x => x.email).EmailAddress().WithMessage("Invalid email format");

        RuleFor(x => x.password).NotEmpty().WithMessage("Password is required");
        RuleFor(x => x.password).MinimumLength(8).WithMessage("Password must be at least 8 characters long");
    }
}

