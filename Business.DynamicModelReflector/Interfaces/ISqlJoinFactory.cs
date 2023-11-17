using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.Enums;
using Business.DynamicModelReflector.Factories;
using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface ISqlJoinFactory<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Builds Query for Left Joins.
        /// </summary>
        /// <param name="leftJoinCondition">Left join Expression.</param>
        /// <returns>ILoadJoinFactory for query manipulation.</returns>
        SqlJoinFactory<TModel> LeftJoin(Expression<Func<TModel, object>> leftJoinCondition);

        /// <summary>
        /// Builds Query for Right Joins.
        /// </summary>
        /// <param name="rightJoinCondition">Right join Expression.</param>
        /// <returns>ILoadJoinFactory for query manipulation.</returns>
        SqlJoinFactory<TModel> RightJoin(Expression<Func<TModel, object>> rightJoinCondition);

        /// <summary>
        /// Builds Query for Inner Joins.
        /// </summary>
        /// <param name="innerJoinCondition">Inner join Expression.</param>
        /// <returns>ILoadJoinFactory for query manipulation.</returns>
        SqlJoinFactory<TModel> InnerJoin(Expression<Func<TModel, object>> innerJoinCondition);

        /// <summary>
        /// Builds the Where Conditions.
        /// </summary>
        /// <param name="whereCondition">Where Condition Expression.</param>
        /// <returns>IWhereFactory for query manipulation.</returns>
        IWhereFactory<TModel> Where(Expression<Func<TModel, bool>> whereCondition);

        /// <summary>
        /// Builds GroupBy Conditions.
        /// </summary>
        /// <param name="groupByCondition">GroupBy Expression.</param>
        /// <returns>IGroupByFactory for query manipulation.</returns>
        IGroupByFactory<TModel> GroupBy(params (Expression<Func<TModel, object>> groupByProperty, SqlAggregateFunctionMenu aggregateFunctionMenu)[] groupByCondition);

        /// <summary>
        /// Builds OrderBy condition.
        /// </summary>
        /// <param name="orderByConditions">Specified properties to order by.</param>
        /// <returns>IExecutable which allows for execution of Query.</returns>
        IExecutable<TModel> OrderBy(params (Expression<Func<TModel, object>> orderByProperty, SqlOrderByMenu orderByMenu)[] orderByConditions);

        /// <summary>
        /// Executes query.
        /// </summary>
        void Execute();
    }
}