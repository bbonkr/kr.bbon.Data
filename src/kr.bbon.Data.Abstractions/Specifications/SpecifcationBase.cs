using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace kr.bbon.Data.Abstractions.Specifications
{

    public abstract class SpecificationBase<TEntity, TResult> : ISpecification<TEntity, TResult>
        where TEntity : class
        where TResult : class
    {
        public SpecificationBase()
        {

        }

        public SpecificationBase(Expression<Func<TEntity, bool>> criteria)
        {
            if (criteria == null)
            {
                throw new ArgumentNullException(nameof(criteria));
            }

            AddCriteria(criteria);
        }

        public IList<Expression<Func<TEntity, bool>>> Criterias { get; } = new List<Expression<Func<TEntity, bool>>>();

        public IList<Expression<Func<TEntity, object>>> Includes { get; } = new List<Expression<Func<TEntity, object>>>();

        public IList<string> IncludePropertyPathStrings { get; } = new List<string>();

        public IList<QueryIncludeInfo<TEntity>> IncludeInfos { get; } = new List<QueryIncludeInfo<TEntity>>();

        public IList<SortInfo> SortDefinitions { get; } = new List<SortInfo>();

        public SortInfo<TEntity> SortByDefinitions { get; private set; } = null;

        public Guid? TenantId { get => tenantId; }

        //public Expression<Func<TEntity, object>> OrderBy { get; private set; }

        //public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

        public Expression<Func<TEntity, object>> GroupBy { get; private set; }
        public bool IsNoTracking => isNoTracking;

        public void AddCriteria(Expression<Func<TEntity, bool>> criteriaExpression)
        {
            if (criteriaExpression == null)
            {
                throw new ArgumentNullException(nameof(criteriaExpression));
            }

            Criterias.Add(criteriaExpression);
        }

        public void AddCriteria(bool condition, Expression<Func<TEntity, bool>> criteriaExpression)
        {
            if (condition)
            {
                AddCriteria(criteriaExpression);
            }
        }

        public void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            if (includeExpression == null)
            {
                throw new ArgumentNullException(nameof(includeExpression));
            }

            Includes.Add(includeExpression);
        }

        public void AddInclude(Expression<Func<TEntity, object>> includeExpression,
            params Expression<Func<object, object>>[] thenIncludeExpression)
        {
            _ = includeExpression ??
                throw new ArgumentException("Include expressions is required", nameof(includeExpression));

            IncludeInfos.Add(new QueryIncludeInfo<TEntity>(includeExpression, thenIncludeExpression));
        }

        public void AddInclude(string navigationPropertyPath)
        {
            if (!string.IsNullOrWhiteSpace(navigationPropertyPath))
            {
                IncludePropertyPathStrings.Add(navigationPropertyPath);
            }
        }

        public void AddSort(SortInfo sortInfo)
        {
            SortDefinitions.Add(sortInfo);
        }

        public void AddOrderBy(Expression<Func<TEntity, object>> expression, bool isAscending = true)
        {
            if (SortByDefinitions == null)
            {
                SortByDefinitions = new SortInfo<TEntity>(expression, isAscending);
            }
        }

        public void AddThenBy(Expression<Func<TEntity, object>> expression, bool isAscending = true)
        {
            if (SortByDefinitions == null)
            {
                throw new NotSupportedException("AddOrderBy를 먼저 사용해야 합니다");
            }

            SortByDefinitions.AddThenBy(expression, isAscending);
        }

        public void AddSort(string fieldName, bool isAscending = true)
        {
            AddSort(new SortInfo
            {
                FieldName = fieldName,
                IsAscending = isAscending
            });
        }

        public void AddGroupBy(Expression<Func<TEntity, object>> groupByExpression)
        {
            if (groupByExpression == null)
            {
                throw new ArgumentNullException(nameof(groupByExpression));
            }

            GroupBy = groupByExpression;
        }

        public void SetNoTracking()
        {
            isNoTracking = true;
        }

        public Expression<Func<TEntity, TResult>> Projection { get; private set; }

        public int? Page { get; private set; }

        public int? Limit { get; private set; }

        public bool CanPagedQuery { get => Page.HasValue && Limit.HasValue; }

        public void AddProject(Expression<Func<TEntity, TResult>> projectionExpression, int? page = null, int? limit = null)
        {
            if (projectionExpression == null)
            {
                throw new ArgumentNullException(nameof(projectionExpression));
            }

            Projection = projectionExpression;

            Page = page;
            Limit = limit;
        }

        public void SetTenantIdFilter(Guid? tenantId)
        {
            this.tenantId = tenantId;
        }

        private bool isNoTracking = false;
        private Guid? tenantId;
    }

    public abstract class SpecificationBase<TEntity> : SpecificationBase<TEntity, TEntity>
        where TEntity : class
    {
        public SpecificationBase() : base()
        {

        }

        public SpecificationBase(Expression<Func<TEntity, bool>> criteria) : base(criteria)
        {

        }
    }

    public class QueryIncludeInfo<TEntity> where TEntity : class
    {
        public Expression<Func<TEntity, object>> Include { get; }

        public IList<Expression<Func<object, object>>> ThenIncludes { get; }

        public QueryIncludeInfo(Expression<Func<TEntity, object>> include, params Expression<Func<object, object>>[] thenIncldes)
        {
            Include = include;
            ThenIncludes = thenIncldes.ToList();
        }
    }
}