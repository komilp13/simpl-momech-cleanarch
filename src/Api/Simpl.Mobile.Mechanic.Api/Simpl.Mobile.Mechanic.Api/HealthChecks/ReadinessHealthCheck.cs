using Microsoft.Extensions.Diagnostics.HealthChecks;
using MongoDB.Bson;
using MongoDB.Driver;
using Simpl.Mobile.Mechanic.DataStore;

namespace Simpl.Mobile.Mechanic.Api.HealthChecks;

public sealed class ReadinessHealthCheck : IHealthCheck
{
    private readonly IMongoHealthChecker _mongoHealthChecker;

    public ReadinessHealthCheck(IMongoHealthChecker mongoHealthChecker)
    {
        _mongoHealthChecker = mongoHealthChecker;
    }

    public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken ct = default)
    {
        var isMongoCheckHealthy = await _mongoHealthChecker.CheckHealthAsync(ct);

        if (isMongoCheckHealthy)
        {
            return HealthCheckResult.Healthy("MongoDB health check successful");
        }
        return HealthCheckResult.Unhealthy("MongoDB health check failed");
    }
}



public interface IMongoHealthChecker
{
    Task<bool> CheckHealthAsync(CancellationToken cancellationToken);
}

internal class MongoHealthChecker : IMongoHealthChecker
{
    private readonly IDbFactory _dbFactory;
    private readonly ILogger<MongoHealthChecker> _logger;

    public MongoHealthChecker(ILogger<MongoHealthChecker> logger, IDbFactory dbFactory)
    {
        _logger = logger;
        _dbFactory = dbFactory;
    }

    public async Task<bool> CheckHealthAsync(CancellationToken cancellationToken = default)
    {
        var isHealthCheckHealthy = await CheckMongoDbconnectionAsync();
        return isHealthCheckHealthy;
    }


    private async Task<bool> CheckMongoDbconnectionAsync()
    {
        try
        {
            IMongoClient dbClient = _dbFactory.GetMongoClient();
            IMongoDatabase db = dbClient.GetDatabase(_dbFactory.Database);
            await db.RunCommandAsync((Command<BsonDocument>)"{ping:1}");
        }
        catch
        {
            return false;
        }

        return true;
    }
}