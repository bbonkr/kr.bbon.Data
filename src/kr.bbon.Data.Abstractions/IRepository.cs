using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using kr.bbon.Core.Models;
using kr.bbon.Data.Abstractions.Specifications;

namespace kr.bbon.Data.Abstractions
{
    public interface IRepository { }

    public interface IRepository<TEntity> : IRepository
    where TEntity : class
    {
        IQueryable<TEntity> GetQuery(ISpecification<TEntity> spec);

        Task<TEntity> GetOneAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

        Task<TModel> GetOneAsync<TModel>(ISpecification<TEntity, TModel> spec, CancellationToken cancellationToken = default) where TModel : class;

        Task<IEnumerable<TEntity>> GetAllAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

        Task<IEnumerable<TModel>> GetAllAsync<TModel>(ISpecification<TEntity, TModel> spec, CancellationToken cancellationToken = default) where TModel : class;

        Task<IPagedModel<TEntity>> GetPagedListAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

        Task<IPagedModel<TResult>> GetPagedListAsync<TResult>(ISpecification<TEntity, TResult> spec, CancellationToken cancellationToken = default) where TResult : class;

        Task<bool> IsExistAsync<TModel>(ISpecification<TEntity, TModel> spec, CancellationToken cancellationToken = default) where TModel : class;

        Task<bool> IsExistAsync(ISpecification<TEntity> spec, CancellationToken cancellationToken = default);

        TEntity Create(TEntity entity);

        TEntity Update(TEntity entity);

        TEntity Delete(TEntity entity);

        void CreateRange(IEnumerable<TEntity> entities);

        void CreateRange(params TEntity[] entities);

        void UpdateRange(IEnumerable<TEntity> entities);

        void UpdateRange(params TEntity[] entities);

        void DeleteRange(IEnumerable<TEntity> entities);

        void DeleteRange(params TEntity[] entities);

        Task<int> SaveAsync(CancellationToken cancellationToken = default);
    }
}