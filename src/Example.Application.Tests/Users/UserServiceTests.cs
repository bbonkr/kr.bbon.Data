using System.Linq;
using System.Threading.Tasks;

using Example.Abstractions;

using Xunit;

namespace Example.Application.Tests.Users;

public class UserServiceTests
{
    public UserServiceTests(UserService userService)
    {
        this.userService = userService;
    }

    [Fact]
    public async Task GetUser_Should_Be_Result()
    {
        var users = await userService.GetAll();

        Assert.NotNull(users);
        Assert.True(users.Count() > 0);
    }

    private readonly UserService userService;
}