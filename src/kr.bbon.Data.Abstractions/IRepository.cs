using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using kr.bbon.Core.Models;

namespace kr.bbon.Data.Abstractions
{
    public interface IRepository { }

    public interface IRepository<TEntity> : IRepository where TEntity : class
    {
        Task<TEntity> GetOneAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

        Task<IPagedModel<TResult>> GetPagedListAsync<TResult>(ISpecification<TEntity, TResult> spec, CancellationToken cancellationToken = default) where TResult : class;

        TEntity Create(TEntity entity);

        TEntity Update(TEntity entity);

        TEntity Delete(TEntity entity);

        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}