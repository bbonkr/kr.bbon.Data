using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using kr.bbon.Core.Models;
using kr.bbon.Data.Abstractions;
using kr.bbon.Data.Abstractions.Specifications;
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

        public virtual Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            return GetAllAsync(spec, cancellationToken);
        }

        public async Task<IEnumerable<TModel>> GetAllAsync<TModel>(ISpecification<TEntity, TModel> spec, CancellationToken cancellationToken = default) where TModel : class
        {
            var result = await context.Set<TEntity>().Specify(spec).ToListAsync(cancellationToken);

            return result;
        }

        public virtual Task<TEntity> GetOneAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            return GetOneAsync(spec, cancellationToken);
        }

        public async Task<TModel> GetOneAsync<TModel>(ISpecification<TEntity, TModel> spec, CancellationToken cancellationToken = default) where TModel : class
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

        public Task<IPagedModel<TEntity>> GetPagedListAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            return GetPagedListAsync(spec, cancellationToken);
        }

        public virtual TEntity Create(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entry = context.Set<TEntity>().Add(entity);

            return entry.Entity;
        }

        public virtual void CreateRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            context.Set<TEntity>().AddRange(entities);
        }

        public virtual void CreateRange(params TEntity[] entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            context.Set<TEntity>().AddRange(entities);
        }

        public virtual TEntity Update(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entry = context.Set<TEntity>().Update(entity);

            return entry.Entity;
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            context.Set<TEntity>().UpdateRange(entities);
        }

        public virtual void UpdateRange(params TEntity[] entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            context.Set<TEntity>().UpdateRange(entities);
        }

        public virtual TEntity Delete(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException(nameof(entity));
            }

            var entry = context.Set<TEntity>().Remove(entity);

            return entry.Entity;
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            context.Set<TEntity>().RemoveRange(entities);
        }

        public virtual void DeleteRange(params TEntity[] entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException(nameof(entities));
            }

            context.Set<TEntity>().RemoveRange(entities);
        }

        public virtual Task<int> SaveAsync(CancellationToken cancellationToken = default)
        {
            return context.SaveChangesAsync(cancellationToken);
        }

        /// <summary>
        /// DO NOT ADD PROJECT in Specification
        /// </summary>
        /// <param name="spec"></param>
        /// <returns></returns>
        public IQueryable<TEntity> GetQuery(ISpecification<TEntity> spec)
        {
            return context.Set<TEntity>().Specify(spec);
        }

        /// <summary>
        /// DO NOT ADD PROJECT in Specification
        /// </summary>
        /// <typeparam name="bool"></typeparam>
        public Task<bool> IsExistAsync<TModel>(ISpecification<TEntity, TModel> spec, CancellationToken cancellationToken = default) where TModel : class
        {
            return context.Set<TEntity>().Specify(spec).AnyAsync(cancellationToken);
        }

        /// <summary>
        /// DO NOT ADD PROJECT in Specification
        /// </summary>
        /// <param name="spec"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task<bool> IsExistAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default)
        {
            return IsExistAsync(spec, cancellationToken);
        }

        protected TDbContext Context { get => context; }
        protected ILogger Logger { get => logger; }

        private readonly TDbContext context;
        private readonly ILogger logger;
    }
}
