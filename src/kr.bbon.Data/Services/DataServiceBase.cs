using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;

using kr.bbon.Data.Abstractions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace kr.bbon.Data.Services
{
    public abstract class DataServiceBase<TDbContext> : IDataService, IDisposable
        where TDbContext : DbContext
    {
        public DataServiceBase(TDbContext context, ILogger logger)
        {
            this.context = context;
            this.logger = logger;
        }

        public void BeginTransaction(IsolationLevel isolationLevel = IsolationLevel.Serializable)
        {
            transaction = context.Database.BeginTransaction(isolationLevel);
        }

        public async Task ComminAsync(CancellationToken cancellationToken = default)
        {
            if (transaction != null)
            {
                await transaction.CommitAsync(cancellationToken);
            }
        }

        public async Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            if (transaction != null)
            {
                await transaction.RollbackAsync(cancellationToken);
            }
        }

        #region IDisposable


        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)

                    if (transaction != null)
                    {
                        transaction.Dispose();
                    }

                    if (context != null)
                    {
                        context.Dispose();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~UnitOfWork()
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
        public TDbContext Context { get => context; }
        protected ILogger Logger { get => logger; }

        private IDbContextTransaction transaction = null;

        private readonly TDbContext context;
        private readonly ILogger logger;
    }
}
