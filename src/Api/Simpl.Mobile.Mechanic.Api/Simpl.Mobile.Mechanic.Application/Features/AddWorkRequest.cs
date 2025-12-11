using System;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Logging;
using Simpl.Mobile.Mechanic.Core;
using Simpl.Mobile.Mechanic.Core.Interfaces.DataRepositories;

namespace Simpl.Mobile.Mechanic.Application.Features.AddWorkRequest;

public record Request : IRequest<BusinessResponse<Response>>
{
    public Guid UserId { get; set; }
    public Int16 Year { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public string Vin { get; set; } = string.Empty;
    public string LicensePlate { get; set; } = string.Empty;
    public string IssueDescription { get; set; } = string.Empty;
}

public class Response
{
    public Guid WorkRequestId { get; set; }
}

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
        // try
        // {
        //     var repoReq = new Core
        //     var resp = await _workRequestRepository.InsertWorkRequestAsync();
        // }
        // catch (Exception ex)
        // {
        //     _logger.LogError(ex, "Error adding work request for user {UserId}", request.UserId);
        //     return ErrorResponse.Create<Response>(new[] { "An error occurred while processing the work request." });
        // }
        throw new NotImplementedException();
    }
}


public class RequestValidator : AbstractValidator<Request>
{
    public RequestValidator()
    {
        RuleFor(r => r.UserId).NotEmpty().WithMessage("UserId is required.");
        RuleFor(r => r.Make).NotEmpty().WithMessage("Vehicle make is required.");
        RuleFor(r => r.Model).NotEmpty().WithMessage("Vehicle model is required.");
        // RuleFor(r => r.Year).InclusiveBetween(1901, DateTime.Now.Year + 1).WithMessage("Vehicle year is invalid.");

        RuleFor(r => r.Vin).NotEmpty().WithMessage("Vehicle VIN is required.");
        RuleFor(r => r.Vin).Length(17).WithMessage("Vehicle VIN must be 17 characters long.");
    }
}