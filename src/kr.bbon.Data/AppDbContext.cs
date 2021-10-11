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
            var assembliesIncludesEntityTypeConfigurationsPredicate = new Func<Type, bool>(t =>
            {
                if (!t.IsClass) { return false; }
                if (t.IsInterface) { return false; }
                if (t.IsAbstract) { return false; }
                if (t == typeof(EntityTypeConfiguration<>)) { return false; }
                if (t == typeof(IEntityType)) { return false; }
                if (!typeof(IEntityType).IsAssignableFrom(t)) { return false; }
                return true;
            });

            var assembliesIncludesEntityTypeConfigurations = ReflectionHelper.CollectAssemblty(t => t != typeof(EntityTypeConfiguration<>) && t != typeof(IEntityType) && typeof(IEntityType).IsAssignableFrom(t));

            foreach (var assembly in assembliesIncludesEntityTypeConfigurations)
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
