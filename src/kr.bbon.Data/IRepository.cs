using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace kr.bbon.Data
{
    public interface IRepository : IDisposable
    {

    }

    public interface IRepository<TEntity> : IRepository where TEntity : class, IEntity
    {
        Task<IQueryable<TEntity>> GetListAsync(Func<TEntity, bool> predicate, CancellationToken cancellationToken = default);

        Task<IQueryable<TEntity>> GetListAsync(Func<TEntity, int, bool> predicate, CancellationToken cancellationToken = default);

        Task<TEntity> FindAsync(Func<TEntity, bool> predicate, CancellationToken cancellationToken = default);
        Task<TEntity> FindAsync(Func<TEntity, int, bool> predicate, CancellationToken cancellationToken = default);

        Task<TEntity> CreateAsync(TEntity entry);

        Task<TEntity> UpdateAsync(TEntity entry);

        Task<TEntity> DeleteAsync(TEntity entry);

        Task BeginTransaction(CancellationToken cancellationToken = default);
        Task<int> SaveAsync(bool autoCommit = true, CancellationToken cancellationToken = default);

        Task CommitAsync(CancellationToken cancellationToken = default);

        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}
