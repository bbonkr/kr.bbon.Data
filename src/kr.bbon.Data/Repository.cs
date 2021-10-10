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

        public async Task BeginTransaction(CancellationToken cancellationToken = default)
        {
            if (transactionCurrent != null)
            {
                throw new Exception("Multiple transaction does not support in current version.");
            }

            transactionCurrent = await dbContext.Database.BeginTransactionAsync(cancellationToken);

            logger.LogInformation($"Transaction {transactionCurrent.TransactionId} was started");
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

        public virtual Task<IQueryable<TEntity>> GetListAsync(Func<TEntity, bool> predicate, CancellationToken cancellationToken = default)
        {
            var query = dbContext.Set<TEntity>().Where(predicate).AsQueryable();

            return Task.FromResult(query);
        }

        public virtual Task<IQueryable<TEntity>> GetListAsync(Func<TEntity, int, bool> predicate, CancellationToken cancellationToken = default)
        {
            var query = dbContext.Set<TEntity>().Where(predicate).AsQueryable();

            return Task.FromResult(query);
        }

        public virtual Task<TEntity> UpdateAsync(TEntity entry)
        {
            throw new NotImplementedException();
        }

        public async virtual Task<int> SaveAsync(bool autoCommit = true, CancellationToken cancellationToken = default)
        {
            try
            {
                var affected = await dbContext.SaveChangesAsync(cancellationToken);
                
                if (autoCommit && transactionCurrent != null)
                {
                    await CommitAsync(cancellationToken);
                }

                return affected;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error occurred when try to save on database.");

                await RollbackAsync(cancellationToken);

                throw;
            }
        }

        public async Task CommitAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (transactionCurrent != null)
                {
                    logger.LogInformation($"Transaction {transactionCurrent.TransactionId} was commited");

                    await transactionCurrent.CommitAsync(cancellationToken);
                }
                else
                {
                    logger.LogInformation($"Transaction {transactionCurrent.TransactionId} did not find");
                }
            }
            finally
            {
                await DisposeTrancationAsync();
            }      
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            try
            {
                if (transactionCurrent != null)
                {
                    await transactionCurrent.RollbackAsync(cancellationToken);

                    logger.LogInformation($"Transaction {transactionCurrent.TransactionId} was rollbacked");
                }
                else
                {
                    logger.LogInformation($"Transaction {transactionCurrent.TransactionId} did not find");
                }
            }
            finally
            {
                await DisposeTrancationAsync();
            }
        }

        private async Task DisposeTrancationAsync()
        {
            if (transactionCurrent != null)
            {
                var transactionId = transactionCurrent.TransactionId;
                await transactionCurrent.DisposeAsync();
                transactionCurrent = null;

                logger.LogInformation($"Transaction {transactionId} was disposed.");
            }
        }


        private readonly AppDbContext dbContext;
        private IDbContextTransaction transactionCurrent = null;
        private ILogger logger;

        #region IDisposable implementation
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)

                    if (transactionCurrent != null)
                    {
                        transactionCurrent.Dispose();
                        transactionCurrent = null;
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~Repository()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
        #endregion


    }
}
