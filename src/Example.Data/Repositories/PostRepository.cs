using Example.Data;
using Example.Entities;

using kr.bbon.Data.Repositories;

using Microsoft.Extensions.Logging;

namespace Example.Data.Repositories
{
    public class PostRepository : RepositoryBase<TestDbContext, Post>
    {
        public PostRepository(TestDbContext context, ILogger<PostRepository> logger) : base(context, logger)
        {
        }
    }
}
