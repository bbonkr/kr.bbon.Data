using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace kr.bbon.Data
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class, IEntity
    {
        public Repository(
            AppDbContext dbContext,
            ILogger logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        /// <summary>
        /// See <see cref="AppDbContext.BeginTransactionAsync(CancellationToken)"/>.
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            await dbContext.BeginTransactionAsync(cancellationToken);
        }

        public virtual Task<TEntity> CreateAsync(TEntity entry)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TEntity> DeleteAsync(TEntity entry)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TEntity> FindAsync(Func<TEntity, bool> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public virtual Task<TEntity> FindAsync(Func<TEntity, int, bool> predicate, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public virtual IQueryable<TEntity> GetList(Func<TEntity, bool> predicate = null)
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (predicate != null)
            {
                query = query.Where(predicate).AsQueryable();
            }

            return query;
        }

        public virtual IQueryable<TEntity> GetList(Func<TEntity, int, bool> predicate)
        {
            IQueryable<TEntity> query = dbContext.Set<TEntity>();

            if (predicate != null)
            {
                query = query.Where(predicate).AsQueryable();
            }

            return query;
        }

        public virtual Task<TEntity> UpdateAsync(TEntity entry)
        {
            throw new NotImplementedException();
        }

        public async virtual Task<int> SaveAsync(bool autoCommit = false, CancellationToken cancellationToken = default)
        {
            try
            {
                var affected = await dbContext.SaveChangesAsync(cancellationToken);
                
                if (autoCommit)
                {
                    await dbContext.CommitTransactionAsync(cancellationToken);
                }

                return affected;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error occurred when try to save on database.");

                await dbContext.RollbackTransactionAsync(cancellationToken);

                throw;
            }
        }

        /// <summary>
        /// See <see cref="AppDbContext.CommitTransactionAsync(CancellationToken)"/>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.CommitTransactionAsync(cancellationToken);  
        }

        /// <summary>
        /// See <see cref="AppDbContext.RollbackTransactionAsync(CancellationToken)"/>
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            return dbContext.RollbackTransactionAsync(cancellationToken);
        }

        private readonly AppDbContext dbContext;
        private ILogger logger;
    }
}
