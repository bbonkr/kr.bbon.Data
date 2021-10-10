using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace kr.bbon.Data
{
    public abstract class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ApplyConfigurationsFromSolution(modelBuilder);
        }

        private void ApplyConfigurationsFromSolution(ModelBuilder modelBuilder)
        {
            var entityTypeConfigurations = AppDomain.CurrentDomain.GetAssemblies()
              .Where(a => a.ExportedTypes.Any(t => t.BaseType != typeof(object) && typeof(IEntityTypeConfiguration<>).IsAssignableFrom(t)));

            foreach (var assembly in entityTypeConfigurations)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            BeforeSaveChanges();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override int SaveChanges()
        {
            return this.SaveChanges(true);
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            return SaveChangesAsync(true, cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            BeforeSaveChanges();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }



        private void BeforeSaveChanges()
        {
            foreach(var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is Entity entryItem)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            entryItem.CreatedAt = DateTimeOffset.UtcNow;
                            entryItem.UpdatedAt = DateTimeOffset.UtcNow;
                            entryItem.IsDeleted = false;
                            break;
                        case EntityState.Modified:
                            entryItem.UpdatedAt = DateTimeOffset.UtcNow;
                            entryItem.IsDeleted = false;
                            break;
                        case EntityState.Deleted:
                            entryItem.DeletedAt = DateTimeOffset.UtcNow;
                            entryItem.IsDeleted = true;
                            break;
                        default:
                            break;
                    }
                }                
            }
        }
    }
}
