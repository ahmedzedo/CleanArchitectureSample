using CleanArchitecture.Application.Common.Exceptions;
using Common.DependencyInjection.Extensions;
using FluentValidation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using CleanArchitecture.Application.Common.Abstracts.Persistence;
using CleanArchitecture.Application.Common.Messaging;
using CleanArchitecture.Application.Common.Abstracts.Business;

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
