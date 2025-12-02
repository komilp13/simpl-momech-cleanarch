using MediatR;
using Simpl.Mobile.Mechanic.Core;

namespace Simpl.Mobile.Mechanic.Application.Features.RegisterUser;


public record Request(string email, string password) : IRequest<BusinessResponse<Response>>;

public class Response
{
  public string Message { get; set; } = string.Empty;
}


public class Handler : IRequestHandler<Request, BusinessResponse<Response>>
{
  public Task<BusinessResponse<Response>> Handle(Request request, CancellationToken ct = default)
  {
    return Task.FromResult(new BusinessResponse<Response>(new Response { Message = "Test response" }, Array.Empty<string>()));
  }
}
