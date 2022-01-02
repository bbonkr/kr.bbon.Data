using Example.Entities;

using kr.bbon.Data.Abstractions;

namespace Example.Abstractions;
public interface IAppDataService : IDataService
{
    public IRepository<User> UserRepository { get; init; }

    public IRepository<Blog> BlogRepository { get; init; }

    public IRepository<Post> PostRepository { get; init; }
}
