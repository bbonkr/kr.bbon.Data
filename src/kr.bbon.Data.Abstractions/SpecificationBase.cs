// using System.Collections.Generic;
// using System.Linq.Expressions;
// using System;
// using kr.bbon.Data.Abstractions;

// namespace kr.bbon.Data.Abstractions
// {

//     public abstract class SpecificationBase<TEntity> : ISpecification<TEntity> where TEntity : class
//     {
//         public SpecificationBase() { }

//         public SpecificationBase(Expression<Func<TEntity, bool>> criteria)
//         {
//             if (criteria == null)
//             {
//                 throw new ArgumentNullException(nameof(criteria));
//             }

//             //Criteria = criteria;
//             AddCriteria(criteria);
//         }

//         public IList<Expression<Func<TEntity, bool>>> Criterias { get; } = new List<Expression<Func<TEntity, bool>>>();

//         public IList<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();

//         public Expression<Func<TEntity, object>> OrderBy { get; private set; }

//         public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

//         public Expression<Func<TEntity, object>> GroupBy { get; private set; }
//         public bool IsNoTracking => isNoTracking;

//         protected void AddCriteria(Expression<Func<TEntity, bool>> criteriaExpression)
//         {
//             if (criteriaExpression == null)
//             {
//                 throw new ArgumentNullException(nameof(criteriaExpression));
//             }

//             Criterias.Add(criteriaExpression);
//         }

//         protected void AddCriteria(bool condition, Expression<Func<TEntity, bool>> criteriaExpression)
//         {
//             if (condition)
//             {
//                 AddCriteria(criteriaExpression);
//             }
//         }

//         protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
//         {
//             if (includeExpression == null)
//             {
//                 throw new ArgumentNullException(nameof(includeExpression));
//             }

//             Includes.Add(includeExpression);
//         }

//         protected void AddSort(Expression<Func<TEntity, object>> orderByExpression, SortOrder sortOrder = SortOrder.Ascending)
//         {
//             if (orderByExpression == null)
//             {
//                 throw new ArgumentNullException(nameof(orderByExpression));
//             }

//             if (sortOrder == SortOrder.Ascending)
//             {
//                 OrderBy = orderByExpression;
//             }
//             else
//             {
//                 OrderByDescending = orderByExpression;
//             }
//         }

//         protected void AddGroupBy(Expression<Func<TEntity, object>> groupByExpression)
//         {
//             if (groupByExpression == null)
//             {
//                 throw new ArgumentNullException(nameof(groupByExpression));
//             }

//             GroupBy = groupByExpression;
//         }

//         public void SetNoTracking()
//         {
//             isNoTracking = true;
//         }

//         private bool isNoTracking = false;
//     }

//     public abstract class SpecificationBase<TEntity, TResult> : SpecificationBase<TEntity>, ISpecification<TEntity, TResult>
//         where TEntity : class
//         where TResult : class
//     {
//         public SpecificationBase() : base() { }

//         public SpecificationBase(Expression<Func<TEntity, bool>> criteria) : base(criteria) { }

//         public Expression<Func<TEntity, int, TResult>> Project { get; private set; }

//         public int? Page { get; private set; }

//         public int? Limit { get; private set; }

//         public bool CanPagedQuery { get => Page.HasValue && Limit.HasValue; }

//         protected void AddProject(Expression<Func<TEntity, int, TResult>> projectExpression, int? page = null, int? limit = null)
//         {
//             if (projectExpression == null)
//             {
//                 throw new ArgumentNullException(nameof(projectExpression));
//             }

//             Project = projectExpression;

//             Page = page;
//             Limit = limit;
//         }
//     }
// }