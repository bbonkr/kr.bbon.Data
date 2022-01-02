using Example.Abstractions;
using Example.Entities;

using Microsoft.Extensions.Logging;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Application
{
    public class UserService
    {
        public UserService(
            IAppDataService dataService,
            ILogger<UserService> logger)
        {
            this.dataService = dataService;
            this.logger = logger;
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            var getUserListSpecification = new GetUserListSpecification();

            var users = await dataService.UserRepository
              .GetAllAsync(getUserListSpecification);

            return users;
        }


        private readonly IAppDataService dataService;
        private readonly ILogger logger;
    }    
}
