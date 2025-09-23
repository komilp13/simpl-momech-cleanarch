using System.Text.RegularExpressions;
using Microsoft.Extensions.Configuration;

namespace Simpl.Mobile.Mechanic.DataStore;

public class MongoSettings
{
    public string ConnectionString { get; }
    public string Database { get; }

    public MongoSettings(IConfiguration config)
    {
        if (config == null)
            throw new ArgumentNullException("config");

        var regex = new Regex(@"(?<server>mongodb(\+srv)?://(.*)/)(?<db>(.*))");
        Match regexMatch = regex.Match(config["MongoConnectionString"] ?? string.Empty);
        if (regexMatch.Success)
        {
            ConnectionString = regexMatch.Groups["server"].Value;
            Database = regexMatch.Groups["db"].Value;
        }
        else
        {
            throw new ArgumentException("Invalid connection string format for mongo database");
        }
        ConnectionString = config["MongoConnectionString"] ?? string.Empty;
    }
}