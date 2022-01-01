using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using kr.bbon.Data.Abstractions;

using Microsoft.EntityFrameworkCore;

namespace kr.bbon.Data.Extensions.DependencyInjection
{
    public static class QueryableExtensions
    {
        public static IQueryable<TEntity> Specify<TEntity>(this IQueryable<TEntity> query, ISpecification<TEntity> spec)
            where TEntity : class
        {
            var isDbQuery = query is DbSet<TEntity>;

            if (spec == null)
            {
                return query;
            }

            if (spec.Criterias.Count > 0)
            {
                query = spec.Criterias.Aggregate(query, (current, criteria) => current.Where(criteria));
            }

            if (isDbQuery)
            {
                if (spec.Includes.Count > 0)
                {
                    query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
                }
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            if (spec.GroupBy != null)
            {
                query = query.GroupBy(spec.GroupBy).SelectMany(x => x);
            }

            if (isDbQuery)
            {
                if (spec.IsNoTracking)
                {
                    query = query.AsNoTracking();
                }
            }
            return query;
        }

        public static IQueryable<TResult> Specify<TEntity, TResult>(this IQueryable<TEntity> query, ISpecification<TEntity, TResult> spec)
            where TEntity : class
            where TResult : class
        {
            if (spec is ISpecification<TEntity> noResultSpec)
            {
                query = query.Specify(noResultSpec);
            }

            if (spec.Project == null)
            {
                throw new ArgumentNullException(nameof(spec.Project), "Project definition must be set.");
            }

            var queryForResult = query.Select(spec.Project);

            return queryForResult;
        }
    }
}
