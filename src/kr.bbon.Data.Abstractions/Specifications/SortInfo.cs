using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace kr.bbon.Data.Abstractions.Specifications
{

    public class SortInfo
    {
        public string FieldName { get; set; }

        public bool IsAscending { get; set; } = true;
    }


    public class SortInfo<TEntity>
    {
        public SortInfo(Expression<Func<TEntity, object>> sort, bool isAscending = true)
        {
            Thens = new List<SortInfo<TEntity>>();

            Sort = sort;
            IsAscending = isAscending;
        }

        public Expression<Func<TEntity, object>> Sort { get; }

        public IList<SortInfo<TEntity>> Thens { get; }

        public bool IsAscending { get; } = true;

        public void AddThenBy(Expression<Func<TEntity, object>> sort, bool isAscending = true)
        {
            Thens.Add(new SortInfo<TEntity>(sort, isAscending));
        }

    }


}