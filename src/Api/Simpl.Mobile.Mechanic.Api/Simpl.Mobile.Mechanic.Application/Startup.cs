using Microsoft.Extensions.DependencyInjection;

namespace Simpl.Mobile.Mechanic.Application;

public static class Startup
{
  public static void Load(IServiceCollection services)
  {
    // load mediatr license
    services.AddMediatR(cfg => {
      cfg.LicenseKey = Environment.GetEnvironmentVariable("MEDIATR_LICENSE_KEY") ?? string.Empty;
      cfg.RegisterServicesFromAssembly(typeof(Startup).Assembly);
    });
  }
}
