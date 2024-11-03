using CleanArchitecture.Infrastructure.Caching.RedisSetupConfigurationOptions;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;
using System.Text;

namespace CleanArchitecture.Infrastructure.Caching;

public static class ConnectionMultiplexerFactory
{
    private static Lazy<ConnectionMultiplexer>? lazyConnection;

    public static ConnectionMultiplexer GetConnection(IConfiguration configuration)
    {
        lazyConnection ??= new Lazy<ConnectionMultiplexer>(() =>
           {
               return ConnectionMultiplexer.ConnectAsync(GetRedisConnectionString(configuration),
                                                         option => option.AbortOnConnectFail = false).Result;
           });

        return lazyConnection.Value;
    }

    private static string GetRedisConnectionString(IConfiguration configuration)
    {
        RedisOptions redisOptions = new();
        RedisOptionsSetup optionSetup = new(configuration);
        optionSetup.Configure(redisOptions);
        StringBuilder connectionStringBuilder = new();
        connectionStringBuilder.Append($"{redisOptions.Host}:{redisOptions.Port}");
        connectionStringBuilder.Append(",abortConnect=false");

        if (string.IsNullOrEmpty(redisOptions.User) && string.IsNullOrEmpty(redisOptions.Password))
        {
            return connectionStringBuilder.ToString();
        }
        connectionStringBuilder.Append($",user={redisOptions.User},password={redisOptions.Password}");

        return connectionStringBuilder.ToString();
    }
}
