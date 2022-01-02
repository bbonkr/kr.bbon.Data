using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using kr.bbon.Core.Models;
using kr.bbon.Data.Abstractions;
using kr.bbon.Data.Extensions.DependencyInjection;
using kr.bbon.EntityFrameworkCore.Extensions;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace kr.bbon.Data.Repositories
{
    public abstract class RepositoryBase<TDbContext, TEntity> : IRepository<TEntity>
        where TDbContext : DbContext
        where TEntity : class
    {
        public RepositoryBase(TDbContext context, ILogger logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            var result = await context.Set<TEntity>().Specify(spec).ToListAsync(cancellationToken);

            return result;
        }

        public virtual async Task<TEntity> GetOneAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            var result = await context.Set<TEntity>().Specify(spec).FirstOrDefaultAsync(cancellationToken);

            return result;
        }

        public async Task<IPagedModel<TResult>> GetPagedListAsync<TResult>(ISpecification<TEntity, TResult> spec, CancellationToken cancellationToken = default)
            where TResult : class
        {
            if (!spec.CanPagedQuery)
            {
                throw new ArgumentException($"For paged query, You must set related fields value; {nameof(spec.Page)}, {nameof(spec.Limit)}.");
            }

            var result = await context.Set<TEntity>().Specify(spec).ToPagedModelAsync(spec.Page ?? -1, spec.Limit ?? -1, cancellationToken);

            return result;
        }

        public virtual TEntity Create(TEntity entity)
        {
            var entry = context.Set<TEntity>().Add(entity);

            return entry.Entity;
        }

        public virtual TEntity Update(TEntity entity)
        {
            var entry = context.Set<TEntity>().Update(entity);

            return entry.Entity;
        }

        public virtual TEntity Delete(TEntity entity)
        {
            var entry = context.Set<TEntity>().Remove(entity);

            return entry.Entity;
        }

        public virtual Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return context.SaveChangesAsync(cancellationToken);
        }

        protected TDbContext Context { get => context; }
        protected ILogger Logger { get => logger; }

        private readonly TDbContext context;
        private readonly ILogger logger;
    }
}
