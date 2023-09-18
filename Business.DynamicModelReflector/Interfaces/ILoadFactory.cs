using Business.DynamicModelReflector.Data.Model;
using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface ILoadFactory<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Builds the columns which will be selected in query.
        /// </summary>
        /// <param name="selectCondition">columns to select.</param>
        /// <returns>ILoadJoinFactory for query manipulation.</returns>
        IJoinFactory<TModel> Select(params Expression<Func<TModel, object>>[] selectCondition);

        /// <summary>
        /// Builds LeftJoin condiotions.
        /// </summary>
        /// <typeparam name="TModelLeft">Left Poco Model.</typeparam>
        /// <typeparam name="TModelRight">Right Poco Model.</typeparam>
        /// <param name="joinCondition">LeftJoin Expression.</param>
        /// <returns>ILoadJoinFactory for query manipulation.</returns>
        IJoinFactory<TModel> LeftJoin<TModelLeft, TModelRight>(Expression<Func<TModelLeft, TModelRight, bool>> joinCondition) where TModelLeft : class, new() where TModelRight : class, new();

        /// <summary>
        /// Builds RightJoin condiotions.
        /// </summary>
        /// <typeparam name="TModelLeft">Left Poco Model.</typeparam>
        /// <typeparam name="TModelRight">Right Poco Model.</typeparam>
        /// <param name="joinCondition">RightJoin Expression.</param>
        /// <returns>ILoadJoinFactory for query manipulation.</returns>
        IJoinFactory<TModel> RightJoin<TModelLeft, TModelRight>(Expression<Func<TModelLeft, TModelRight, bool>> joinCondition) where TModelLeft : class, new() where TModelRight : class, new();

        /// <summary>
        /// Builds InnerJoin condiotions.
        /// </summary>
        /// <typeparam name="TModelLeft">Left Poco Model.</typeparam>
        /// <typeparam name="TModelRight">Right Poco Model.</typeparam>
        /// <param name="joinCondition">InnerJoin Expression.</param>
        /// <returns>ILoadJoinFactory for query manipulation.</returns>
        IJoinFactory<TModel> InnerJoin<TModelLeft, TModelRight>(Expression<Func<TModelLeft, TModelRight, bool>> joinCondition) where TModelLeft : class, new() where TModelRight : class, new();

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