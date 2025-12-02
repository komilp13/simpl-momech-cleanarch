using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Simpl.Mobile.Mechanic.Api.Endpoints;
using Simpl.Mobile.Mechanic.Api.HealthChecks;
using Simpl.Mobile.Mechanic.DataStore;

var builder = WebApplication.CreateBuilder(args);

var mongoSettings = new MongoSettings(builder.Configuration);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// Register Health checks
builder.Services.AddHealthChecks()
    .AddCheck<LivenessHealthCheck>("liveness")
    .AddCheck<ReadinessHealthCheck>("readiness");

    // Load dependency chain
Simpl.Mobile.Mechanic.Application.Startup.Load(builder.Services);


#region Register Service Dependencies

builder.Services.AddTransient<MongoSettings>((a) => mongoSettings);

builder.Services.AddTransient<IMongoHealthChecker, MongoHealthChecker>();
builder.Services.AddScoped<IDbFactory, DbFactory>();

#endregion


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

#region Define Health Checks

app.MapHealthChecks("/alivez", new HealthCheckOptions()
{
    Predicate = c => c.Name == "liveness"
});

app.MapHealthChecks("/readyz", new HealthCheckOptions()
{
    Predicate = c => c.Name == "readiness"
});

#endregion

#region Endpoints

UserEndpoints.Register(app);

#endregion

app.Run();