using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace CleanArchitecture.Infrastructure.Caching.RedisSetupConfigurationOptions;

#region Redis Option
public class RedisOptions
{
    public string? User { get; init; }
    public string? Host { get; init; }
    public int Port { get; init; }
    public string? Password { get; init; }
    public bool Ssl { get; init; }

    public static explicit operator ConfigurationOptions(RedisOptions options) => new()
    {
        EndPoints = { { options.Host ?? throw new InvalidOperationException("Redis Host can't be null"), options.Port } },
        User = options.User,
        Password = options.Password,
        Ssl = options.Ssl
    };

}
#endregion

#region Redis Option Setup
public class RedisOptionsSetup : IConfigureOptions<RedisOptions>
{
    #region Properties
    private const string SECTION_NAME = "Redis";
    private IConfiguration Configuration { get; }
    #endregion

    #region Constuctor
    public RedisOptionsSetup(IConfiguration configuration)
    {
        Configuration = configuration;
    }
    #endregion

    #region Configure
    public void Configure(RedisOptions options)
    {
        Configuration.GetSection(SECTION_NAME).Bind(options);
    }
    #endregion
}
#endregion
