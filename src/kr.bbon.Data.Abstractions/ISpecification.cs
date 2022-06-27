// using System.Collections.Generic;
// using System.Linq.Expressions;
// using System;

// namespace kr.bbon.Data.Abstractions
// {
//     public interface ISpecification { }

//     public interface ISpecification<TEntity> : ISpecification where TEntity : class
//     {
//         IList<Expression<Func<TEntity, bool>>> Criterias { get; }

//         IList<Expression<Func<TEntity, object>>> Includes { get; }

//         Expression<Func<TEntity, object>> OrderBy { get; }

//         Expression<Func<TEntity, object>> OrderByDescending { get; }

//         Expression<Func<TEntity, object>> GroupBy { get; }

//         bool IsNoTracking { get; }

//         void SetNoTracking();
//     }

//     public interface ISpecification<TEntity, TResult> : ISpecification<TEntity>
//         where TEntity : class
//         where TResult : class
//     {
//         Expression<Func<TEntity, int, TResult>> Project { get; }

//         int? Page { get; }

//         int? Limit { get; }

//         bool CanPagedQuery { get; }
//     }
// }