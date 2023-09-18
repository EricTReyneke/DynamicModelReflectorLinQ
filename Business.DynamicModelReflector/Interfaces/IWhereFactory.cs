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
        IGroupByFactory<TModel> GroupBy(params (Expression<Func<TModel, object>> groupByProperty, AggregateFunctionMenu aggregateFunctionMenu)[] groupByCondition);

        /// <summary>
        /// Builds OrderBy condition.
        /// </summary>
        /// <param name="orderByConditions">Specified properties to order by.</param>
        /// <returns>IExecutable which allows for execution of Query.</returns>
        IExecutable<TModel> OrderBy(params (Expression<Func<TModel, object>> orderByProperty, OrderByMenu orderByMenu)[] orderByConditions);

        /// <summary>
        /// Executes query.
        /// </summary>
        void Execute();
    }
}