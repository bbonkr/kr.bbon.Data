using System;
using System.Linq.Expressions;

namespace kr.bbon.Data.Abstractions.Specifications
{

    public class Specification<TEntity> : SpecificationBase<TEntity> where TEntity : class
    {
        public Specification() : base()
        {
        }

        public Specification(Expression<Func<TEntity, bool>> criteria) : base(criteria)
        {
        }
    }

    public class Specification<TEntity, TResult> : SpecificationBase<TEntity, TResult> where TEntity : class where TResult : class
    {
        public Specification() : base()
        {
        }

        public Specification(Expression<Func<TEntity, bool>> criteria) : base(criteria)
        {
        }
    }
}