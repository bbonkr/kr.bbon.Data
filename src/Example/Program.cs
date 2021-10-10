using kr.bbon.Data;
using kr.bbon.Data.Tests.Example;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Linq;
using System.Threading.Tasks;

namespace Example
{
    class Program
    {
        static Task Main(string[] args) => CreateHostBuilder(args).Build().RunAsync();

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureServices(services =>
            {
                services.Configure<DatabaseOptions>(options =>
                {
                    options.UseSoftDelete = true;
                });

                services.AddDbContext<TestDbContext>(options =>
                {
                    options.UseSqlite("data source=test.db"
                    //    , sqliteOptions =>
                    //{
                    //    sqliteOptions.MigrationsAssembly(this.GetType().Name);
                    //}
                    );
                }, ServiceLifetime.Singleton);

                services.AddHostedService<DatabaseMigrationService>();
                
            });
          
    }
}
