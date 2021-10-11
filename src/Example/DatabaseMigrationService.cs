using kr.bbon.Data.Tests.Example;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Example
{
    public class DatabaseMigrationService : IHostedService
    {
        public DatabaseMigrationService(
            TestDbContext dbContext, 
            IHostLifetime hostLifetime,
            UserRepository userRepository
            )
        {
            this.dbContext = dbContext;
            this.hostLifetime = hostLifetime;
            this.userRepository = userRepository;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await hostLifetime.WaitForStartAsync(cancellationToken);

            await dbContext.Database.MigrateAsync(cancellationToken);

            var users = await userRepository.GetList()
                .Include(x => x.Blogs)
                .Include(x => x.Posts)
                .Select(x => new { x.UserName })
                .OrderBy(x => x.UserName)
                .ToListAsync(cancellationToken);

            Console.WriteLine($"Users {users.Count}");

            await hostLifetime.StopAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private readonly TestDbContext dbContext;
        private readonly IHostLifetime hostLifetime;
        private readonly UserRepository userRepository;
    }
}
