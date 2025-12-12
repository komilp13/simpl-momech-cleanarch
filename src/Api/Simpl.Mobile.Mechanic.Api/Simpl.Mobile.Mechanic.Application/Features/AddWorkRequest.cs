using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Simpl.Mobile.Mechanic.Core;
using Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories;

namespace Simpl.Mobile.Mechanic.Application.Features.AddWorkRequest;

#region Request

public record Request(Guid UserId, Address Address, VehicleInfo VehicleInfo, string IssueDescription)
    : IRequest<BusinessResponse<Response>>;

public record Address(string Addr1, string Addr2, string City, string State, string ZipCode);

public record VehicleInfo(Int16 Year, string Make, string Model, string Vin, string LicensePlate);

#endregion

public record Response(Ulid WorkRequestId);

public class Handler : IRequestHandler<Request, BusinessResponse<Response>>
{
    private readonly ILogger<Handler> _logger;
    private readonly IWorkRequestRepository _workRequestRepository;

    public Handler(ILogger<Handler> logger, IWorkRequestRepository workRequestRepo)
    {
        _logger = logger;
        _workRequestRepository = workRequestRepo;
    }

    public async Task<BusinessResponse<Response>> Handle(Request request, CancellationToken ct)
    {
        // validate data
        var validator = new RequestValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => e.ErrorMessage).ToArray();
            return ErrorResponse.Create<Response>(errors);
        }

        // save work request to database
        try
        {
            var repoReq = new Core
            var resp = await _workRequestRepository.InsertWorkRequestAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error adding work request for user {UserId}", request.UserId);
            return ErrorResponse.Create<Response>(new[] { "An error occurred while processing the work request." });
        }
    }
}

#region Validators
    
internal class RequestValidator : AbstractValidator<Request>
{
    public RequestValidator()
    {
        RuleFor(r => r.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(r => r.IssueDescription).NotEmpty().WithMessage("IssueDescription is required.");

        RuleFor(r => r.Address).NotNull().WithMessage("Address is required.").SetValidator(new AddressValidator());
        RuleFor(r => r.VehicleInfo).NotNull().WithMessage("VehicleInfo is required").SetValidator(new VehicleValidator());
    }
}

internal class AddressValidator : AbstractValidator<Address>
{
    public AddressValidator()
    {
        RuleFor(r => r.Addr1).NotEmpty().WithMessage("Addr1 is required.");
        RuleFor(r => r.Addr2).NotEmpty().WithMessage("Addr2 is required.");
        RuleFor(r => r.City).NotEmpty().WithMessage("City is required.");
        RuleFor(r => r.State).NotEmpty().WithMessage("State is required.");
        RuleFor(r => r.ZipCode).NotEmpty().WithMessage("ZipCode is required.");
    }
}

internal class VehicleValidator : AbstractValidator<VehicleInfo>
{
    public VehicleValidator()
    {
        RuleFor(r => r.Year).InclusiveBetween((Int16)1901, (Int16)(DateTime.Now.Year + 1)).WithMessage("Invalid Vehicle Year.");
        RuleFor(r => r.Make).NotEmpty().WithMessage("Make is required.");
        RuleFor(r => r.Model).NotEmpty().WithMessage("Model is required.");
        RuleFor(r => r.Vin).NotEmpty().WithMessage("Vin is required.");
        RuleFor(r => r.LicensePlate).NotEmpty().WithMessage("LicensePlate is required.");
    }
}

#endregion