using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using kr.bbon.Core.Reflection;
using kr.bbon.Data.Abstractions.Entities;
using kr.bbon.Data.Extensions.DependencyInjection;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace kr.bbon.Data
{
    public abstract class AppDbContextBase : DbContext
    {
        public AppDbContextBase(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    base.OnModelCreating(modelBuilder);

        //    ApplyConfigurationsFromSolution(modelBuilder);
        //}

        protected virtual void ApplyConfigurationsFromAssemblies(ModelBuilder modelBuilder, IEnumerable<Assembly> assemblies = null)
        {
            //var assembliesIncludesEntityTypeConfigurationsPredicate = new Func<Type, bool>(t =>
            //{
            //    if (!t.IsClass) { return false; }
            //    if (t.IsInterface) { return false; }
            //    if (t.IsAbstract) { return false; }
            //    if (t == typeof(EntityTypeConfiguration<>)) { return false; }
            //    if (t == typeof(IEntityType)) { return false; }
            //    if (!typeof(IEntityType).IsAssignableFrom(t)) { return false; }
            //    return true;
            //});

            //var assembliesIncludesEntityTypeConfigurations = ReflectionHelper.CollectAssembly(t => t != typeof(EntityTypeConfiguration<>) && t != typeof(IEntityType) && typeof(IEntityType).IsAssignableFrom(t));
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

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            if (transaction != null)
            {
                throw new Exception($"Multiple transaction does not support in current version. (transaction id: {transaction.TransactionId})");
            }

            transaction = await Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (transaction != null)
                {
                    await transaction.CommitAsync(cancellationToken);
                }
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (transaction != null)
                {
                    await transaction.RollbackAsync(cancellationToken);
                }
            }
            finally
            {
                await DisposeTransactionAsync();
            }
        }

        private async Task DisposeTransactionAsync()
        {
            if (transaction != null)
            {
                await transaction.DisposeAsync();
                transaction = null;
            }
        }

        public override void Dispose()
        {
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }

            base.Dispose();
        }

        public override ValueTask DisposeAsync()
        {
            if (transaction != null)
            {
                transaction.Dispose();
                transaction = null;
            }

            return base.DisposeAsync();
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

        private IDbContextTransaction transaction;
    }
}
