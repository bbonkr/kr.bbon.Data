using kr.bbon.Core.Reflection;
using kr.bbon.Data.Abstractions;
using kr.bbon.Data.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

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

        public static IServiceCollection AddDatabaseOptions(this IServiceCollection services, Action<DatabaseOptions> databaseOptionsAction = null)
        {
            if (databaseOptionsAction == null)
            {
                services.AddOptions<DatabaseOptions>().Configure<IConfiguration>((options, configuration) =>
                {
                    var sectionValue = configuration.GetSection(DatabaseOptions.Name).Value;
                    if (!string.IsNullOrWhiteSpace(sectionValue))
                    {
                        configuration.GetSection(DatabaseOptions.Name).Bind(options);
                    }
                    else
                    {
                        options.UseSoftDelete = false;
                    }
                });
            }
            else
            {
                services.Configure<DatabaseOptions>(databaseOptionsAction);
            }

            return services;
        }

        //    public static IServiceCollection AddGenericRepositories(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped )
        //    {
        //        var predicate = new Func<Type, bool>(t =>
        //        {
        //            if (!t.IsClass) { return false; }
        //            if (t.IsInterface) { return false; }
        //            if (t.IsAbstract) { return false; }
        //            if (t == typeof(IRepository)) { return false; }
        //            if (t == typeof(IRepository<>)) { return false; }
        //            if (t == typeof(Repository<>)) { return false; }
        //            if (!typeof(IRepository).IsAssignableFrom(t)) { return false; }

        //            return true;
        //        });

        //        var types = ReflectionHelper.CollectTypes( predicate);

        //        foreach (var type in types)
        //        {
        //            switch (serviceLifetime)
        //            {
        //                case ServiceLifetime.Singleton:
        //                    services.AddSingleton(type);
        //                    break;
        //                case ServiceLifetime.Transient:
        //                    services.AddTransient(type);
        //                    break;
        //                default:
        //                    services.AddScoped(type);
        //                    break;
        //            }
        //        }

        //        return services;
        //    }

        //    public static IServiceCollection AddAppDbContext<TDbContext>(this IServiceCollection services, 
        //        Action<IServiceProvider,DbContextOptionsBuilder> optionsAction,
        //        ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime  = ServiceLifetime.Scoped) where TDbContext : AppDbContext
        //    {
        //        services.Add(new ServiceDescriptor(typeof(AppDbContext), typeof(TDbContext)));

        //        services.AddDbContext<TDbContext>(optionsAction, contextLifetime, optionsLifetime);

        //        return services;
        //    }

        //    public static IServiceCollection AddAppDbContext<TDbContext>(this IServiceCollection services,
        //        Action<DbContextOptionsBuilder> optionsAction,
        //        ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TDbContext : AppDbContext
        //    {
        //        services.Add(new ServiceDescriptor(typeof(AppDbContext), typeof(TDbContext)));

        //        services.AddDbContext<TDbContext>(optionsAction, contextLifetime, optionsLifetime);

        //        return services;
        //    }
    }
}
