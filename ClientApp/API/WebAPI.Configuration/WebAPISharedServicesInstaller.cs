using CleanArchitecture.Application.Common.Abstracts.Business;
using CleanArchitecture.Application.Common.Messaging;
using Common.DependencyInjection.Extensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.WebAPI.Configuration
{
    public class WebApiSharedServicesInstaller : IServiceInstaller
    {
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            services.AddValidatorsFromAssembly(AppDomain.CurrentDomain.GetAssembly(nameof(Application)));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssembly(nameof(Application)));
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(AppDomain.CurrentDomain.GetAssembly(nameof(Application))));
            services.RegisterAllForBaseDynamic(ServiceLifetime.Transient, nameof(Application), typeof(IRequestResponsePipeline<,>));
            services.RegisterAllForBaseDynamic(ServiceLifetime.Transient, nameof(Application), typeof(IRequestPreProcessor<>));
            services.RegisterAllForBaseDynamic(ServiceLifetime.Transient, nameof(Application), typeof(IRequestPostProcessor<>));
            services.RegisterAllChildsDynamic(ServiceLifetime.Scoped, nameof(Application), typeof(IService));
        }
    }
}
