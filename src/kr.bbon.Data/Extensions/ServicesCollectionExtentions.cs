using kr.bbon.Core.Reflection;
using kr.bbon.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace kr.bbon.Data
{
    public static class ServicesCollectionExtentions
    {
        public static IServiceCollection AddDatabaseOptions(this IServiceCollection services)
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

            return services;
        }

        public static IServiceCollection AddGenericRepositories(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Scoped )
        {
            var predicate = new Func<Type, bool>(t =>
            {
                if (!t.IsClass) { return false; }
                if (t.IsInterface) { return false; }
                if (t.IsAbstract) { return false; }
                if (t == typeof(IRepository)) { return false; }
                if (t == typeof(IRepository<>)) { return false; }
                if (t == typeof(Repository<>)) { return false; }
                if (!typeof(IRepository).IsAssignableFrom(t)) { return false; }

                return true;
            });

            var types = ReflectionHelper.CollectTypes( predicate);

            foreach (var type in types)
            {
                switch (serviceLifetime)
                {
                    case ServiceLifetime.Singleton:
                        services.AddSingleton(type);
                        break;
                    case ServiceLifetime.Transient:
                        services.AddTransient(type);
                        break;
                    default:
                        services.AddScoped(type);
                        break;
                }
            }

            return services;
        }

        public static IServiceCollection AddAppDbContext<TDbContext>(this IServiceCollection services, 
            Action<IServiceProvider,DbContextOptionsBuilder> optionsAction,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime  = ServiceLifetime.Scoped) where TDbContext : AppDbContext
        {
            services.Add(new ServiceDescriptor(typeof(AppDbContext), typeof(TDbContext)));

            services.AddDbContext<TDbContext>(optionsAction, contextLifetime, optionsLifetime);

            return services;
        }

        public static IServiceCollection AddAppDbContext<TDbContext>(this IServiceCollection services,
            Action<DbContextOptionsBuilder> optionsAction,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped) where TDbContext : AppDbContext
        {
            services.Add(new ServiceDescriptor(typeof(AppDbContext), typeof(TDbContext)));

            services.AddDbContext<TDbContext>(optionsAction, contextLifetime, optionsLifetime);

            return services;
        }
    }
}
