using System;
using System.Linq;

using kr.bbon.Data.Abstractions.Specifications;
using kr.bbon.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace kr.bbon.Data.Extensions.DependencyInjection
{
    public static class QueryableExtensions
    {
        public static IQueryable<TResult> Specify<TEntity, TResult>(this IQueryable<TEntity> query, ISpecification<TEntity, TResult> spec)
                where TEntity : class
                where TResult : class
        {
            if (spec == null)
            {
                throw new ArgumentNullException();
            }

            var isDbQuery = query is DbSet<TEntity>;

            // if (spec.TenantId.HasValue && typeof(TEntity).IsAssignableTo(typeof(IHasTenantId)))
            // {
            //     query.Where(x => (x as IHasTenantId).TenantId == spec.TenantId);
            // }

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

                if (spec.IncludePropertyPathStrings.Count > 0)
                {
                    query = spec.IncludePropertyPathStrings.Aggregate(query, (current, propertyPath) => current.Include(propertyPath));
                }

                if (spec.IncludeInfos.Count > 0)
                {
                    foreach (var include in spec.IncludeInfos)
                    {
                        query = query.Include(include.Include);
                        if (include.ThenIncludes.Count > 0)
                        {
                            foreach (var thenInclude in include.ThenIncludes)
                            {
                                if (query is IIncludableQueryable<TEntity, object> includeQuery)
                                {
                                    query = includeQuery.ThenInclude(thenInclude);
                                }
                            }
                        }
                        // else
                        // {
                        //     query = query.Include(include.Include);
                        // }
                    }
                }
            }

            if (spec.SortDefinitions.Count > 0)
            {
                foreach (var sort in spec.SortDefinitions)
                {
                    query = query.Sort(sort.FieldName, sort.IsAscending);

                }
            }

            if (spec.SortByDefinitions != null)
            {
                if (spec.SortByDefinitions.IsAscending)
                {
                    query = query.OrderBy(spec.SortByDefinitions.Sort);
                }
                else
                {
                    query = query.OrderByDescending(spec.SortByDefinitions.Sort);
                }

                if (spec.SortByDefinitions.Thens != null && spec.SortByDefinitions.Thens.Count > 0)
                {

                    foreach (var then in spec.SortByDefinitions.Thens)
                    {
                        if (query.Expression.Type == typeof(IOrderedQueryable<TEntity>))
                        {
                            var orderedQueryable = query as IOrderedQueryable<TEntity>;

                            if (then.IsAscending)
                            {
                                query = orderedQueryable.ThenBy(then.Sort);
                            }
                            else
                            {
                                query = orderedQueryable.ThenByDescending(then.Sort);
                            }
                        }
                    }
                }
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

            return query.Select(spec.Projection);
        }

        public static IQueryable<TEntity> Specify<TEntity>(this IQueryable<TEntity> query, ISpecification<TEntity> spec)
            where TEntity : class
        {
            return query.Specify<TEntity, TEntity>(spec);
        }

        // public static IQueryable<TEntity> Specify<TEntity>(this IQueryable<TEntity> query, ISpecification<TEntity> spec)
        //     where TEntity : class
        // {
        //     var isDbQuery = query is DbSet<TEntity>;

        //     if (spec == null)
        //     {
        //         return query;
        //     }

        //     if (spec.Criterias.Count > 0)
        //     {
        //         query = spec.Criterias.Aggregate(query, (current, criteria) => current.Where(criteria));
        //     }

        //     if (isDbQuery)
        //     {
        //         if (spec.Includes.Count > 0)
        //         {
        //             query = spec.Includes.Aggregate(query, (current, include) => current.Include(include));
        //         }
        //     }

        //     if (spec.OrderBy != null)
        //     {
        //         query = query.OrderBy(spec.OrderBy);
        //     }
        //     else if (spec.OrderByDescending != null)
        //     {
        //         query = query.OrderByDescending(spec.OrderByDescending);
        //     }

        //     if (spec.GroupBy != null)
        //     {
        //         query = query.GroupBy(spec.GroupBy).SelectMany(x => x);
        //     }

        //     if (isDbQuery)
        //     {
        //         if (spec.IsNoTracking)
        //         {
        //             query = query.AsNoTracking();
        //         }
        //     }
        //     return query;
        // }

        // public static IQueryable<TResult> Specify<TEntity, TResult>(this IQueryable<TEntity> query, ISpecification<TEntity, TResult> spec)
        //     where TEntity : class
        //     where TResult : class
        // {
        //     if (spec is ISpecification<TEntity> noResultSpec)
        //     {
        //         query = query.Specify(noResultSpec);
        //     }

        //     if (spec.Project == null)
        //     {
        //         throw new ArgumentNullException(nameof(spec.Project), "Project definition must be set.");
        //     }

        //     var queryForResult = query.Select(spec.Project);

        //     return queryForResult;
        // }
    }
}
