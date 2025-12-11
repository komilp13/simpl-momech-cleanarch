using MongoDB.Driver;

namespace Simpl.Mobile.Mechanic.DataStore;

public interface IDbFactory
{
    string Database { get; }

    IMongoClient GetMongoClient();
}

public sealed class DbFactory : IDbFactory
{
    private readonly MongoSettings _settings;

    public DbFactory(MongoSettings settings)
    {
        _settings = settings;
    }
    public string Database => _settings.Database;

    public IMongoClient GetMongoClient()
    {
        return new MongoClient(_settings.ConnectionString);
    }
}