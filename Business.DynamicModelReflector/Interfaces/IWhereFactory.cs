using Business.DynamicModelReflector.Data.Model;
using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface IWhereFactory<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Builds GroupBy Conditions.
        /// </summary>
        /// <param name="groupByCondition">GroupBy Expression.</param>
        /// <returns>IGroupByFactory for query manipulation.</returns>
        IGroupByFactory<TModel> GroupBy(Expression<Func<TModel, object>> groupByCondition);

        /// <summary>
        /// Builds OrderBy condition.
        /// </summary>
        /// <param name="orderByCondition">OrderBy Expression.</param>
        /// <returns>IExecutable which allows for execution of Query.</returns>
        IExecutable<TModel> OrderBy(Expression<Func<TModel, OrderByMenu>> orderByCondition);

        /// <summary>
        /// Executes query.
        /// </summary>
        void Execute();
    }
}