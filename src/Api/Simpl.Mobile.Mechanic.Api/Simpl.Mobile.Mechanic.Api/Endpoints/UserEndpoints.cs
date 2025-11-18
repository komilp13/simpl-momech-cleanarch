using Simpl.Mobile.Mechanic.Core.Mediator;

namespace Simpl.Mobile.Mechanic.Api.Endpoints;

public static class UserEndpoints
{
  public static void Register(WebApplication app)
  {
    app.MapPost("/register", async (ISender sender, CancellationToken ct) =>
    {
      var result = await sender.Send(new Application.RegisterUser.Request(), ct);
      return Results.Ok(result.Message);
    });
  }
}
