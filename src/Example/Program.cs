
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using Example.DbContexts;
using Example.HostedServices;
using Example.Services;

using kr.bbon.Data.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Example
{
    class Program
    {
        static Task Main(string[] args) => CreateHostBuilder(args).Build().RunAsync();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                var assembly = typeof(TestDbContext).Assembly;

                services.AddLogging();

                services.AddRepositories(new List<Assembly> { assembly }, ServiceLifetime.Scoped);
                services.AddDataService<DataService>();

                services.AddDbContext<TestDbContext>(options =>
                {
                    options.UseSqlite(
                        "data source=test.db",
                        sqliteOptions =>
                        {
                            sqliteOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                            sqliteOptions.MigrationsAssembly(assembly.FullName);
                        });
                });

                services.AddHostedService<DatabaseMigrationService>();
            });

    }
}
