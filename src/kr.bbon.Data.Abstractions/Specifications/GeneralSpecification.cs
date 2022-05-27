using System;
using System.Linq.Expressions;

namespace kr.bbon.Data.Abstractions.Specifications
{

    public class GeneralSpecification<TEntity> : SpecificationBase<TEntity> where TEntity : class
    {
        public GeneralSpecification() : base()
        {

        }

        public GeneralSpecification(Expression<Func<TEntity, bool>> criteria) : base(criteria)
        {
        }
    }

    public class GeneralSpecification<TEntity, TModel> : SpecificationBase<TEntity, TModel>
        where TEntity : class
        where TModel : class
    {
        public GeneralSpecification() : base()
        {

        }
    }
}