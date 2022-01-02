using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Example.Abstractions;
using Example.Application.Tests.Mocks;

using Microsoft.Extensions.DependencyInjection;

using Moq;

namespace Example.Application.Tests
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();

            services.AddScoped<IAppDataService>(_ => AppDataServiceMock.Create().Object);

            services.AddScoped<UserService>();
        }
    }
}
