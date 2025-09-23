using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Simpl.Mobile.Mechanic.Api.HealthChecks;

public sealed class LivenessHealthCheck : IHealthCheck
{
    public Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        return Task.FromResult(HealthCheckResult.Healthy("alive"));
    }
}