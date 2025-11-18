using Simpl.Mobile.Mechanic.Core.Mediator;

namespace Simpl.Mobile.Mechanic.Application.RegisterUser;


public class Request : IRequest<Response>
{

}

public class Response
{
  public string Message { get; set; } = string.Empty;
}


public class Handler : IRequestHandler<Request, Response>
{
  public Task<Response> Handle(Request request, CancellationToken ct = default)
  {
    return Task.FromResult<Response>(new Response { Message = "Test response" });
  }
}
