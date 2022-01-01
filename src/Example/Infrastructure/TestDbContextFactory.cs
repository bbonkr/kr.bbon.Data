
using Example.DbContexts;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Example
{
    public class TestDbContextFactory : IDesignTimeDbContextFactory<TestDbContext>
    {
        public TestDbContext CreateDbContext(string[] args)
        {
            var connectionString = "data source=test.db";

            var dbContextOptionsBuilder = new DbContextOptionsBuilder<TestDbContext>();
            dbContextOptionsBuilder.UseSqlite(connectionString, sqlServerOptions =>
            {
                sqlServerOptions.MigrationsAssembly(typeof(TestDbContext).Assembly.FullName);
            });

            var dbContextOptions = dbContextOptionsBuilder.Options;

            return new TestDbContext(dbContextOptions);
        }
    }
}
