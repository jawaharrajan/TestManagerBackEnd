using System.Linq.Expressions;

namespace TestManager.DataAccess.Sort
{
    public static class QueryableSortExtensions
    {
        public static IQueryable<T> ApplySorting<T, TEnum>(
        this IQueryable<T> query,
             List<(TEnum Field, bool Descending)> sortFields,
            Dictionary<TEnum, Expression<Func<T, object>>> fieldMap)
            where TEnum : System.Enum
        {
            if (sortFields == null || sortFields.Count == 0)
                return query;

            IOrderedQueryable<T>? ordered = null;

            foreach (var (field, desc) in sortFields)
            {
                if (!fieldMap.TryGetValue(field, out var expression))
                    continue;

                if (ordered == null)
                {
                    ordered = desc
                        ? query.OrderByDescending(expression)
                        : query.OrderBy(expression);
                }
                else
                {
                    ordered = desc
                        ? ordered.ThenByDescending(expression)
                        : ordered.ThenBy(expression);
                }
            }

            return ordered ?? query;
        }
    }

}
