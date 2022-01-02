using Example.Data;
using Example.Entities;

using kr.bbon.Data.Repositories;

using Microsoft.Extensions.Logging;

namespace Example.Data.Repositories
{
    public class BlogRepository : RepositoryBase<TestDbContext, Blog>
    {
        public BlogRepository(TestDbContext context, ILogger<BlogRepository> logger) : base(context, logger)
        {
        }
    }
}
