using Example.Data;
using Example.Entities;

using kr.bbon.Data.Repositories;

using Microsoft.Extensions.Logging;

namespace Example.Data.Repositories
{
    public class UserRepository : RepositoryBase<TestDbContext, User>
    {
        public UserRepository(TestDbContext context, ILogger<UserRepository> logger) : base(context, logger)
        {
        }
    }
}
