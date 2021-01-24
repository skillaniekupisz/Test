using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Games.Infrastructure.Repositories.InMemory
{
    public static class EnumerableExtensions
    {
        public static IQueryable<T> Where<T>(this IQueryable<T> objects, Func<bool> condition, Expression<Func<T, bool>> predicate) where T : class
        {
            if (condition())
            {
                return objects.Where(predicate);
            }

            return objects;
        }

        public static IList<T> GetPage<T>(this IEnumerable<T> objects, int? pageSize, int? pageIndex) where T : class
        {
            var size = pageSize ?? 10;
            var index = pageIndex ?? 1;

            return objects.Skip((index - 1) * size).Take(size).ToList();
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> objects, string orderProperty, bool isDescending = false)
        {
            if (string.IsNullOrWhiteSpace(orderProperty))
                return objects;

            var command = isDescending ? "OrderByDescending" : "OrderBy";
            var type = typeof(T);
            var property = type.GetProperty(orderProperty);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExpression = Expression.Lambda(propertyAccess, parameter);
            var resultExpression = Expression.Call(typeof(Queryable), command, new[] { type, property.PropertyType }, objects.Expression, Expression.Quote(orderByExpression));

            return objects.Provider.CreateQuery<T>(resultExpression);
        }
    }
}
