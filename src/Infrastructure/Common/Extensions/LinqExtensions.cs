using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CYRetailIMS.Infrastructure.Common.Extensions;
public static class LinqExtensions
{
    public static IQueryable<T> OrderByDynamic<T>(this IQueryable<T> source, string propertyName, bool ascending)
    {
        var parameter = Expression.Parameter(typeof(T), "x");
        var selector = Expression.PropertyOrField(parameter, propertyName);
        var method = ascending ? "OrderBy" : "OrderByDescending";
        var expression = Expression.Lambda(selector, parameter);
        var result = Expression.Call(typeof(Queryable), method, new Type[] { typeof(T), selector.Type },
                                     source.Expression, Expression.Quote(expression));
        return source.Provider.CreateQuery<T>(result);
    }

}
