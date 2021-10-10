using kr.bbon.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
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
    }
}
