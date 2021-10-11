using Example.Entities;
using Microsoft.Extensions.Logging;

namespace kr.bbon.Data.Tests.Example
{
    public class UserRepository : Repository<User>
    {
        public UserRepository(
            TestDbContext dbContext,
            ILogger<UserRepository> logger)
            : base(dbContext, logger)
        {

        }
    }
}
