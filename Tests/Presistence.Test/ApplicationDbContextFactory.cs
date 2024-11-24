using CleanArchitecture.Application.Common.Abstracts.Account;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Persistence.EF;
using CleanArchitecture.Persistence.EF.Interceptors;
using Common.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using static System.Net.Mime.MediaTypeNames;

namespace Presistence.Test
{
    public class ApplicationDbContextFactory
    {
        private readonly bool _useRealDatabase;
        public IApplicationDbContext DbContext { get; }
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public ApplicationDbContextFactory()
        {
            _configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json")
                                                       .Build();
            _useRealDatabase = _configuration.GetValue<bool>("UseRealDatabase");
            var services = new ServiceCollection();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssembly(nameof(Application))));
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
               CreateDbContext(options)
              .AddInterceptors(sp.GetServices<ISaveChangesInterceptor>()));
            services.AddScoped<IApplicationDbContext>(s => s.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<IDbContext>(s => s.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
            services.RegisterAllChildsDynamic(ServiceLifetime.Transient, nameof(Application), nameof(CleanArchitecture.Persistence.EF), typeof(IEntitySet<>));
            services.AddSingleton<ICurrentUser, CurrentUser>();
            _serviceProvider = services.BuildServiceProvider();
            DbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();
            services.AddScoped<ApplicationDbContextInitializer>();
        }
        public DbContextOptionsBuilder CreateDbContext(DbContextOptionsBuilder options)
        {
            if (_useRealDatabase)
            {
                // Use real SQL Server database
                options.UseSqlServer(_configuration.GetConnectionString("DefaultConnection"),
                                   b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName));
            }
            else
            {
                // Use in-memory database
                options.UseInMemoryDatabase("CLeanArchitectureDbTest");
            }

            return options;
        }
    }
}
