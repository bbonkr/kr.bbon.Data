using Example.Services;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Example.HostedServices
{
    public class DatabaseMigrationService : IHostedService
    {
        public DatabaseMigrationService(
            IHostLifetime hostLifetime,
            DataService dataService,
            ILogger<DatabaseMigrationService> logger)
        {
            this.hostLifetime = hostLifetime;
            this.dataService = dataService;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await hostLifetime.WaitForStartAsync(cancellationToken);

            await dataService.Context.Database.MigrateAsync(cancellationToken);

            var getUserListSpecification = new GetUserListSpecification();

            var users = await dataService.UserRepository
                .GetAllAsync(getUserListSpecification);

            logger.LogInformation("Users {count}", users.Count());

            await hostLifetime.StopAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }


        private readonly IHostLifetime hostLifetime;
                private readonly DataService dataService;
        private readonly ILogger logger;
    }    
}
