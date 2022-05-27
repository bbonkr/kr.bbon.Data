using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Example.Entities;

using kr.bbon.Data.Abstractions;
using kr.bbon.Data.Abstractions.Specifications;
using Moq;

namespace Example.Application.Tests.Mocks
{
    public class UserRepositoryMock
    {
        public static Mock<IRepository<User>> Create()
        {
            var users = new List<User>
            {
                new User{
                    Id = 1,
                    UserName = "Test #1",
                    IsDeleted= false,
                },
                new User{
                    Id = 2,
                    UserName = "Test #2",
                    IsDeleted= false,
                },
            };

            var userRepositoryMock = new Mock<IRepository<User>>();

            userRepositoryMock
                .Setup(x => x.GetAllAsync(It.IsAny<ISpecification<User>>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(() => users.AsEnumerable());

            return userRepositoryMock;
        }
    }
}
