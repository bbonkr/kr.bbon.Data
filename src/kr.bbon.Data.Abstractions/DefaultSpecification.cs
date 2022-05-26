using System.Linq.Expressions;
using System;

namespace kr.bbon.Data.Abstractions
{

    public class DefaultSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class
    {
        public DefaultSpecification() : base()
        {

        }

        public DefaultSpecification(Expression<Func<TEntity, bool>> criteria) : base(criteria)
        {

        }
    }

    public class DefaultSpecification<TEntity, TResult> : SpecificationBase<TEntity, TResult>
        where TEntity : class
        where TResult : class
    {
        public DefaultSpecification() : base()
        {

        }

        public DefaultSpecification(Expression<Func<TEntity, bool>> criteria) : base(criteria)
        {

        }
    }
}