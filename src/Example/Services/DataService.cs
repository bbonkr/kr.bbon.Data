using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Example.DbContexts;
using Example.Entities;

using kr.bbon.Data.Abstractions;
using kr.bbon.Data.Services;

using Microsoft.Extensions.Logging;

namespace Example.Services
{
    public class DataService : DataServiceBase<TestDbContext>
    {
        public DataService(
            TestDbContext context,
            ILogger<DataService> logger,
            IRepository<User> userRepository,
            IRepository<Blog> blogRepository,
            IRepository<Post> postRepository)
            : base(context, logger)
        {
            UserRepository = userRepository;
            BlogRepository = blogRepository;
            PostRepository = postRepository;
        }

        public IRepository<User> UserRepository { get;  init; }

        public IRepository<Blog> BlogRepository { get; init; }

        public IRepository<Post> PostRepository { get; init; }
    }
}
