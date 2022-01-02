using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Example.Abstractions;

using Moq;

namespace Example.Application.Tests.Mocks
{
    public class AppDataServiceMock
    {
        public static Mock<IAppDataService> Create()
        {
            var appDataServiceMock = new Mock<IAppDataService>();

            appDataServiceMock.Setup(x => x.UserRepository).Returns(UserRepositoryMock.Create().Object);

            return appDataServiceMock;
        }
    }
}
