using System;
using System.Linq.Expressions;

namespace kr.bbon.Data.Abstractions.Specifications
{
    public class SpecificationBuilder<TEntity, TResult>
        where TEntity : class
        where TResult : class
    {
        public SpecificationBuilder()
        {
            _specification = new Specification<TEntity, TResult>();
        }

        public Specification<TEntity, TResult> Build()
        {
            if (_specification.Projection is null)
            {
                if (typeof(TEntity) == typeof(TResult))
                {
                    _specification.AddProject((x) => x as TResult);
                }
                else
                {
                    throw new NotSupportedException("프로젝션 정의가 누락되었습니다.");
                }
            }

            return _specification;
        }

        public SpecificationBuilder<TEntity, TResult> AddCriteria(Expression<Func<TEntity, bool>> criteriaExpression)
        {
            _specification.AddCriteria(criteriaExpression);

            return this;
        }

        public SpecificationBuilder<TEntity, TResult> AddCriteria(bool condition, Expression<Func<TEntity, bool>> criteriaExpression)
        {
            _specification.AddCriteria(condition, criteriaExpression);

            return this;
        }

        public SpecificationBuilder<TEntity, TResult> AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {
            _specification.AddInclude(includeExpression);

            return this;
        }

        /// <summary>
        /// 관련 데이터 로드 정의를 구성합니다 
        /// </summary>
        /// <param name="navigationPropertyPath">"." 으로 구분된 속성 이름 탐색 경로입니다.</param>
        /// <returns></returns>
        public SpecificationBuilder<TEntity, TResult> AddInclude(string navigationPropertyPath)
        {
            _specification.AddInclude(navigationPropertyPath);

            return this;
        }

        public SpecificationBuilder<TEntity, TResult> AddInclude(Expression<Func<TEntity, object>> includeExpression,
            params Expression<Func<object, object>>[] thenIncludeExpression)
        {
            _specification.AddInclude(includeExpression, thenIncludeExpression);

            return this;
        }

        //public SpecificationBuilder<TEntity, TResult> AddSort(SortInfo sortInfo)
        //{
        //    _specification.AddSort(sortInfo);

        //    return this;
        //}

        //public SpecificationBuilder<TEntity, TResult> AddSort(string fieldName, bool isAscending = true)
        //{
        //    _specification.AddSort(fieldName, isAscending);

        //    return this;
        //}

        public SpecificationBuilder<TEntity, TResult> AddOrderBy(Expression<Func<TEntity, object>> sortExpression, bool isAscending = true)
        {
            _specification.AddOrderBy(sortExpression, isAscending);

            return this;
        }

        public SpecificationBuilder<TEntity, TResult> AddThenBy(Expression<Func<TEntity, object>> sortExpression, bool isAscending = true)
        {
            _specification.AddThenBy(sortExpression, isAscending);

            return this;
        }

        public SpecificationBuilder<TEntity, TResult> AddGroupBy(Expression<Func<TEntity, object>> groupByExpression)
        {
            _specification.AddGroupBy(groupByExpression);

            return this;
        }

        public SpecificationBuilder<TEntity, TResult> SetNoTracking()
        {
            _specification.SetNoTracking();

            return this;
        }

        public SpecificationBuilder<TEntity, TResult> AddProject(Expression<Func<TEntity, TResult>> projectionExpression, int? page = null, int? limit = null)
        {
            _specification.AddProject(projectionExpression, page, limit);

            return this;
        }

        public SpecificationBuilder<TEntity, TResult> AddProject(Expression<Func<TEntity, int, TResult>> projectionExpression, int? page = null, int? limit = null)
        {
            _specification.AddProject(t => projectionExpression.Compile().Invoke(t, 0), page, limit);

            return this;
        }

        public SpecificationBuilder<TEntity, TResult> SetTenantIdFilter(Guid? tenantId)
        {
            if (tenantId.HasValue)
            {
                _specification.SetTenantIdFilter(tenantId);
            }

            return this;
        }

        protected readonly Specification<TEntity, TResult> _specification;
    }

    public class SpecificationBuilder<TEntity> : SpecificationBuilder<TEntity, TEntity> where TEntity : class
    {
        public SpecificationBuilder() : base()
        {

        }
    }
}