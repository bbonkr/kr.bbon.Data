
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

using Example.Abstractions;
using Example.Application;
using Example.Application.Services;
using Example.Data;
using Example.Data.Seeders;
using Example.Services;

using kr.bbon.Data.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Example
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var app = CreateHostBuilder(args).Build();

            //using (var scope = app.Services.CreateScope())
            //{
            //    var context = scope.ServiceProvider.GetRequiredService<TestDbContext>();
            //    var userSeeder = scope.ServiceProvider.GetRequiredService<UserSeeder>();

            //    await context.Database.MigrateAsync();

            //    await userSeeder.SeedAsync();
            //}

            var context = app.Services.GetRequiredService<TestDbContext>();
            var userSeeder = app.Services.GetRequiredService<TestDataSeeder>();

            await context.Database.MigrateAsync();

            await userSeeder.SeedAsync();

            await app.RunAsync();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                var assembly = typeof(TestDbContext).Assembly;

                services.AddLogging();

                services.AddRepositories(new List<Assembly> { assembly }, ServiceLifetime.Singleton);
                services.AddDataService<IAppDataService, AppDataService>(ServiceLifetime.Singleton);

                services.AddDbContext<TestDbContext>(options =>
                {
                    options.UseSqlite(
                        "data source=test.db",
                        sqliteOptions =>
                        {
                            sqliteOptions.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
                            sqliteOptions.MigrationsAssembly(typeof(Example.Data.Sqlite.PlaceHolder).Assembly.FullName);
                        });
                }, ServiceLifetime.Singleton);

                services.AddSingleton<UserService>();
                services.AddSingleton<TestDataSeeder>();

                services.AddHostedService<DataBaseQueryService>();
            });
    }
}
