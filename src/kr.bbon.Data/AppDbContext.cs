using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using kr.bbon.Data.Abstractions.Entities;

using Microsoft.EntityFrameworkCore;

namespace kr.bbon.Data
{
    public abstract class AppDbContextBase : DbContext
    {
        public AppDbContextBase(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        protected virtual void ApplyConfigurationsFromAssemblies(ModelBuilder modelBuilder, IEnumerable<Assembly> assemblies = null)
        {
            var assembliesIncludesEntityTypeConfigurationsPredicate = GetPrdicateForFliteringEntityTypeConfigurationInAssembly();

            foreach (var assembly in assemblies)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(assembly, assembliesIncludesEntityTypeConfigurationsPredicate);
            }
        }

        protected virtual Func<Type, bool> GetPrdicateForFliteringEntityTypeConfigurationInAssembly()
        {
            return new Func<Type, bool>(t =>
            {
                if (t.IsInterface) { return false; }
                if (!t.IsClass) { return false; }
                if (t.IsAbstract) { return false; }
                if (t == typeof(EntityTypeConfigurationBase<>)) { return false; }
                if (t == typeof(IHasEntityType)) { return false; }

                var result = false;

                var baseType = t.DeclaringType?.BaseType ?? t.BaseType;

                if (baseType != null && baseType != typeof(Object))
                {
                    if (baseType.IsGenericType)
                    {
                        result = baseType.GetGenericTypeDefinition() == typeof(EntityTypeConfigurationBase<>);
                    }
                }

                return result;
            });
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            BeforeSaveChanges();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override int SaveChanges()
        {
            return SaveChanges(true);
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

        protected virtual void BeforeSaveChanges()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is EntitySupportSoftDeletionBase entryItem)
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
