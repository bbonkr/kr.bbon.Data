using Example.Data.Repositories;
using Example.Entities;
using kr.bbon.Data.Abstractions;
using kr.bbon.Data.Abstractions.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Example.Data.Seeders;

public class TestDataSeeder
{
    public TestDataSeeder(
        //IRepository<User> userRepository,
        //IRepository<Blog> blogRepository,
        //IRepository<Post> postRepository
        //UserRepository userRepository
        TestDbContext dbContext
        )
    {
        //_userRepository = userRepository;
        //_blogRepository = blogRepository;
        //_postRepository = postRepository;
        _dbContext = dbContext;
    }
    public async Task SeedAsync()
    {
        //var specification = new SpecificationBuilder<User>()
        //    .Build();

        //var isExist = await _userRepository.IsExistAsync(specification);
        var isExist = await _dbContext.Users.AnyAsync();
        if (!isExist)
        {
            var testUser = new User
            {
                UserName = "Test user",
                //IsDeleted = false,
                Blogs = new List<Blog> {
                    new Blog{
                        Title = "Test blog",
                        //IsDeleted = false,
                        //Posts = new List<Post>{
                        //    new Post{Title = "Test post #1", Content = "Hello #1", IsDeleted = false,},
                        //    new Post{Title = "Test post #2", Content = "Hello #2", IsDeleted = false, },
                        //}
                    }
                },
            };

            _dbContext.Users.Add(testUser);
            await _dbContext.SaveChangesAsync();

            var addedUser = await _dbContext.Users
                .Include(x => x.Blogs)
                .FirstOrDefaultAsync();


            //var blog = new Blog
            //{
            //    Title = "Test blog",
            //    OwnerId = addedUser.Id,
            //};

            //var addedBlog = _blogRepository.Create(blog);
            if (addedUser != null)
            {
                var addedBlog = addedUser.Blogs.First();

                var posts = new List<Post>
            {
                new Post{Title = "Test post #1", Content = "Hello #1", AuthorId = addedUser.Id, BlogId = addedBlog.Id, },
                new Post{Title = "Test post #2", Content = "Hello #2",AuthorId = addedUser.Id, BlogId = addedBlog.Id, },
            };

                _dbContext.Posts.AddRange(posts);
                await _dbContext.SaveChangesAsync();
            }
            //_postRepository.CreateRange(posts);

            //await _userRepository.SaveAsync();
        }
    }

    //private readonly IRepository<User> _userRepository;
    //private readonly IRepository<Blog> _blogRepository;
    //private readonly IRepository<Post> _postRepository;

    private readonly TestDbContext _dbContext;

    //private readonly UserRepository _userRepository;
}