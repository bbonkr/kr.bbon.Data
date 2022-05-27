using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace kr.bbon.Data.Abstractions.Specifications
{

    public interface ISpecification<TEntity, TResult>
        where TEntity : class
        where TResult : class
    {
        IList<Expression<Func<TEntity, bool>>> Criterias { get; }

        IList<Expression<Func<TEntity, object>>> Includes { get; }

        IList<string> IncludePropertyPathStrings { get; }

        IList<QueryIncludeInfo<TEntity>> IncludeInfos { get; }

        IList<SortInfo> SortDefinitions { get; }

        SortInfo<TEntity> SortByDefinitions { get; }

        Expression<Func<TEntity, object>> GroupBy { get; }

        Expression<Func<TEntity, TResult>> Projection { get; }

        bool IsNoTracking { get; }

        Guid? TenantId { get; }

        void SetNoTracking();

        int? Page { get; }

        int? Limit { get; }

        bool CanPagedQuery { get; }
    }

    public interface ISpecification<TEntity> : ISpecification<TEntity, TEntity>
        where TEntity : class
    {

    }
}