using Common.DependencyInjection.Exceptions;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace Common.DependencyInjection.Extensions
{
    public enum FilterNameType
    {
        Equal,
        Prefix,
        Suffix,
        Contains
    }
    public static class ServiceCollectionDynamicRegisterExtension
    {
        #region Enums
        enum DependecyType
        {
            Interface,
            Class,
        }

        #endregion

        #region Public Methods 
        public static IServiceCollection AddDependeciesDynamic(this IServiceCollection services,
                                                               ServiceLifetime serviceLifetime,
                                                               string abstractNamespace,
                                                               string ImplementatioNamespace,
                                                               string filterName,
                                                               FilterNameType filterNameType)
        {
            var interfaces = GetAssemblyNamedTypes(abstractNamespace, DependecyType.Interface, filterName, filterNameType);

            if (interfaces != null && interfaces.Count != 0)
            {
                var classes = GetAssemblyNamedTypes(ImplementatioNamespace, DependecyType.Class, filterName, filterNameType);

                if (classes != null && classes.Count != 0)
                {
                    services.RegisterTypesBasedOnDefaultNaming(serviceLifetime, interfaces, classes);
                }
            }

            return services;
        }

        public static IServiceCollection AddDependeciesDynamic(this IServiceCollection services,
                                                               ServiceLifetime serviceLifetime,
                                                               string nameSpace,
                                                               string filterName,
                                                               FilterNameType filterNameType)
        {
            return services.AddDependeciesDynamic(serviceLifetime, nameSpace, nameSpace, filterName, filterNameType);
        }
        public static IServiceCollection RegisterAllChildsDynamic(this IServiceCollection services,
                                                                  ServiceLifetime serviceLifetime,
                                                                  string abstractNamespaceName,
                                                                  string ImplementationamespaceName,
                                                                  Type baseType)
        {
            var interfaces = GetAssemblyTypeChilds(abstractNamespaceName, DependecyType.Interface, baseType);

            if (interfaces != null)
            {
                services = interfaces!.Aggregate(services, (serv, i) => RegisterAllForSingleBaseDynamic(serv, serviceLifetime, ImplementationamespaceName, i));
            }

            return services;
        }
        public static IServiceCollection RegisterAllChildsDynamic(this IServiceCollection services,
                                                                  ServiceLifetime serviceLifetime,
                                                                  string nameSpace,
                                                                  Type baseType)
        {
            return services.RegisterAllChildsDynamic(serviceLifetime, nameSpace, nameSpace, baseType);
        }

        public static IServiceCollection RegisterAllForSingleBaseDynamic(this IServiceCollection services,
                                                                  ServiceLifetime serviceLifetime,
                                                                  string ImplementationamespaceName,
                                                                  Type baseType)
        {
            var classes = GetAssemblyTypeChilds(ImplementationamespaceName, DependecyType.Class, baseType);

            if (classes != null && classes.Count != 0)
            {
                classes.ForEach(c => services.AddToServices(serviceLifetime, baseType, c));
            }

            return services;
        }

        public static IEnumerable<T> GetInstances<T>(this IServiceProvider serviceProvider) => (IEnumerable<T>)serviceProvider.GetServices(typeof(T)) ?? throw new UnregisteredServiceException(nameof(T));
        public static T GetInstance<T>(this IServiceProvider serviceProvider) => (T?)serviceProvider.GetService(typeof(T)) ?? throw new UnregisteredServiceException(nameof(T));
        public static Assembly GetAssembly(this AppDomain appDomain, string assemblyName) => Array.Find(appDomain.GetAssemblies(), a => a.FullName != null && a.FullName.Contains(assemblyName))
                                        ?? throw new NotFoundAssmblyException(assemblyName);
        #endregion

        #region Helper Methods
        private static List<Type>? GetAssemblyNamedTypes(string namespaceName,
                                                         DependecyType dependecyType,
                                                         string filterName,
                                                         FilterNameType filterNameType)
        {
            return GetAssembly(AppDomain.CurrentDomain, namespaceName)?.GetTypes()?.AsEnumerable()
                                                                       .FilterByDependecyType(dependecyType)
                                                                       .FilterByName(filterName, filterNameType)
                                                                       .ToList();
        }
        private static List<Type>? GetAssemblyTypeChilds(string namespaceName,
                                                         DependecyType dependecyType,
                                                         Type type)
        {
            return GetAssembly(AppDomain.CurrentDomain, namespaceName)?.GetTypes()?.AsEnumerable()
                                                                       .FilterChildsByDependecyType(dependecyType, type)
                                                                       .ToList();
        }

        private static IServiceCollection RegisterTypesBasedOnDefaultNaming(this IServiceCollection services,
                                                        ServiceLifetime serviceLifetime,
                                                        List<Type> interfaces,
                                                        List<Type> classes)
        {
            interfaces.ForEach(delegate (Type i)
            {
                var nameComparer = i.Name[1..].ToString();
                var implement = classes.Find(c => c.Name == nameComparer);

                if (implement != null)
                {
                    services.AddToServices(serviceLifetime, i, implement);
                }
            });

            return services;
        }

        private static IServiceCollection AddToServices(this IServiceCollection services,
                                                        ServiceLifetime serviceLifetime,
                                                        Type interfaceType,
                                                        Type implement)
        {
            return serviceLifetime switch
            {
                ServiceLifetime.Singleton => services.AddSingleton(interfaceType, implement),
                ServiceLifetime.Scoped => services.AddScoped(interfaceType, implement),
                ServiceLifetime.Transient => services.AddTransient(interfaceType, implement),
                _ => services,
            };
        }
        private static IEnumerable<Type> FilterByName(this IEnumerable<Type> types,
                                                      string filterWord,
                                                      FilterNameType filterNameType)
        {
            if (!string.IsNullOrEmpty(filterWord))
            {
                types = filterNameType switch
                {
                    FilterNameType.Equal => types.Where(t => t.Name == filterWord),
                    FilterNameType.Prefix => types.Where(t => (t.IsInterface
                                                                 && t.Name[1..]
                                                                     .ToLower()
                                                                     .StartsWith(filterWord.ToLower()))
                                                                 || t.Name.ToLower()
                                                                 .StartsWith(filterWord.ToLower())),
                    FilterNameType.Suffix => types.Where(t => t.Name.ToLower()
                                                                    .EndsWith(filterWord.ToLower())),
                    FilterNameType.Contains => types.Where(t => t.Name.Contains(filterWord.ToLower())),
                    _ => types.Where(t => t.Name.Contains(filterWord)),
                };
            }

            return types;
        }

        private static IEnumerable<Type> FilterByDependecyType(this IEnumerable<Type> types,
                                                               DependecyType dependecyType)
        {
            return dependecyType switch
            {
                DependecyType.Interface => types.Where(t => t.IsInterface || t.IsAbstract || t.ContainsGenericParameters),
                DependecyType.Class => types.Where(t => t.IsClass || t.IsAbstract || t.ContainsGenericParameters),
                _ => types,
            };
        }

        private static IEnumerable<Type> FilterChildsByDependecyType(this IEnumerable<Type> types,
                                                                     DependecyType dependecyType,
                                                                     Type baseType)
        {
            var childs = types.Where(t => baseType.IsAssignableFrom(t)
                                       || Array.Exists(t.GetInterfaces(), i => i.IsGenericType && i.GetGenericTypeDefinition() == baseType));


            return dependecyType switch
            {
                DependecyType.Interface => childs.Where(t => t.IsInterface && t != baseType).ToList(),
                DependecyType.Class => childs.Where(t => t.IsClass && !t.IsAbstract).ToList(),
                _ => childs
            };

        }

        #endregion


    }
}
