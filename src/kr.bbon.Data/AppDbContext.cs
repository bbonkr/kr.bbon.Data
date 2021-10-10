using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
            var entityTypeConfigurations = new List<Assembly>();
            
            var allAssembly = AppDomain.CurrentDomain.GetAssemblies();

            foreach (var assembly in allAssembly)
            {
                if (assembly.IsDynamic)
                {
                    continue;
                }

                var foundTypes = assembly
                    .GetExportedTypes()
                    //.Where(t => t != typeof(EntityTypeConfiguration<>) && t != typeof(IEntityType) && typeof(IEntityType).IsAssignableFrom(t));
                    .Where(t => t != typeof(EntityTypeConfiguration<>))
                    .Where(t => t != typeof(IEntityType))
                    .Where(t => typeof(IEntityType).IsAssignableFrom(t));

                if (foundTypes.Count() > 0)
                {
                    entityTypeConfigurations.Add(assembly);
                }
            }

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
