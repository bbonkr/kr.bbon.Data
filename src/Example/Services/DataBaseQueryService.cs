using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Application.Services
{
    public class DataBaseQueryService : IHostedService
    {
        public DataBaseQueryService(
            IHostLifetime hostLifetime,
            UserService userService,
            ILogger<DataBaseQueryService> logger)
        {
            this.hostLifetime = hostLifetime;
            this.userService = userService;
            this.logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await hostLifetime.WaitForStartAsync(cancellationToken);

            var users = await userService.GetAll();

            logger.LogInformation("Users {count}", users.Count());

            await hostLifetime.StopAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }


        private readonly IHostLifetime hostLifetime;
        private readonly UserService userService;
        private readonly ILogger logger;
    }    
}
