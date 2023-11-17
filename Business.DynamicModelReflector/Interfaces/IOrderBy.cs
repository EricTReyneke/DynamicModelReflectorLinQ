using Business.DynamicModelReflector.Data.Model;
using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface IOrderBy<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Builds OrderBy condition.
        /// </summary>
        /// <param name="orderByConditions">Specified properties to order by.</param>
        void OrderBy(params (Expression<Func<TModel, object>> orderByProperty, OrderByMenu orderByMenu)[] orderByConditions);
    }
}