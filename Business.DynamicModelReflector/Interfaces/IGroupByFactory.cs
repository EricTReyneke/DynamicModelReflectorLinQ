using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.Enums;
using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface IGroupByFactory<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Builds OrderBy condition.
        /// </summary>
        /// <param name="propertiesToOrderBy">Specified properties to order by.</param>
        /// <returns>IExecutable which allows for execution of Query.</returns>
        IExecutable<TModel> OrderBy(params (Expression<Func<TModel, object>> orderByProperty, SqlOrderByMenu orderByMenu)[] propertiesToOrderBy);

        /// <summary>
        /// Executes query.
        /// </summary>
        void Execute();
    }
}