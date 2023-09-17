using Business.DynamicModelReflector.Data.Model;
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
        IExecutable<TModel> OrderBy(params (Func<TModel, object> orderByProperty, OrderByMenu orderByMenu)[] propertiesToOrderBy);

        /// <summary>
        /// Executes query.
        /// </summary>
        void Execute();
    }
}