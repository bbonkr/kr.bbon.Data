namespace kr.bbon.Data.Abstractions
{

    public class SpecificationFactory
    {
        public ISpecification<TEntity> Create<TEntity>() where TEntity : class
        {
            return new DefaultSpecification<TEntity>();
        }

        public ISpecification<TEntity, TResult> Create<TEntity, TResult>()
            where TEntity : class
            where TResult : class
        {
            return new DefaultSpecification<TEntity, TResult>();
        }
    }
}