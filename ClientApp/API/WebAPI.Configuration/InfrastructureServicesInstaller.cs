using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Persistence.EF;
using CleanArchitecture.Persistence.EF.Interceptors;
using Common.DependencyInjection.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.WebAPI.Configuration
{
    public class PresistenceServicesInstaller : IServiceInstaller
    {
        public PresistenceServicesInstaller()
        {
        }

        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
               options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"),
                                    b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName))
               .AddInterceptors(sp.GetServices<ISaveChangesInterceptor>()));
            //services.AddScoped<IDbContext>(s => s.GetRequiredService<ApplicationDbContext>());
            //services.AddScoped<IApplicationDbContext>(s => s.GetRequiredService<ApplicationDbContext>());
            services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();
            services.AddScoped<ISaveChangesInterceptor, DispatchDomainEventsInterceptor>();
            services.RegisterAllChildsDynamic(ServiceLifetime.Transient, nameof(Application), nameof(Persistence.EF), typeof(IEntitySet<>));
            services.AddScoped<ApplicationDbContextInitializer>();
        }

    }
}
