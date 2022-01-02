using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using kr.bbon.Data.Abstractions;

using Microsoft.Extensions.DependencyInjection;

namespace kr.bbon.Data.Extensions.DependencyInjection
{
    public static class ServicesCollectionExtentions
    {
        public static IServiceCollection AddDataService<TDataService>(this IServiceCollection services) where TDataService : class, IDataService
        {
            services.AddScoped<TDataService>();

            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services, IEnumerable<Assembly> assemblies, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped)
        {
            foreach (var assembly in assemblies)
            {
                var exportedTypes = assembly.GetExportedTypes();
                var types = exportedTypes.Where(t => t.IsClass && !t.IsAbstract && t.IsPublic && !t.IsInterface && t.IsAssignableTo(typeof(IRepository)));

                foreach (var repositoryType in types)
                {
                    if (repositoryType != null && repositoryType.BaseType != null && repositoryType.BaseType.GenericTypeArguments != null && repositoryType.BaseType.GenericTypeArguments.Length > 1)
                    {
                        var genericArgumentType = repositoryType.BaseType.GenericTypeArguments[1];

                        var serviceType = typeof(IRepository<>).MakeGenericType(genericArgumentType);

                        switch (serviceLifetime)
                        {
                            case ServiceLifetime.Singleton:
                                services.AddSingleton(serviceType, repositoryType);
                                break;
                            case ServiceLifetime.Transient:
                                services.AddTransient(serviceType, repositoryType);
                                break;
                            default:
                                services.AddScoped(serviceType, repositoryType);
                                break;
                        }
                    }
                }
            }

            return services;
        }
    }
}
