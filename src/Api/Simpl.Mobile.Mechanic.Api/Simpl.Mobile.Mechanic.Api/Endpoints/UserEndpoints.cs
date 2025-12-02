using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Simpl.Mobile.Mechanic.Api.Endpoints;

public static class UserEndpoints
{
  public static void Register(WebApplication app)
  {
    app.MapPost("/register", async ([FromBody] Application.Features.RegisterUser.Request request, ISender sender, CancellationToken ct) =>
    {
      var result = await sender.Send(request, ct);
      return Results.Ok(result.Data?.Message);
    });
  }
}
