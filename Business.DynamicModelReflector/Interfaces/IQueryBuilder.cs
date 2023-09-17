using System.Data.SqlClient;
using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface IQueryBuilder
    {
        /// <summary>
        /// Builds Where Conditions.
        /// </summary>
        /// <typeparam name="TModel">Generic Poco Model.</typeparam>
        /// <param name="whereCondition">Where Condition Expression.</param>
        /// <returns>Where Condition String.</returns>
        string BuildWhereConditions<TModel>(Expression<Func<TModel, bool>> whereCondition);

        /// <summary>
        /// Builds Left Join Conditions.
        /// </summary>
        /// <typeparam name="TModelLeft">Generic Poco object of the Left Table.</typeparam>
        /// <typeparam name="TModelRight">Generic Poco object of the Right Table.</typeparam>
        /// <param name="leftJoinCondition">Left Join Condition Expression.</param>
        /// <returns>Left Join Conditions in string Format.</returns>
        string BuildLeftJoinConditions<TModelLeft, TModelRight>(Expression<Func<TModelLeft, TModelRight, bool>> leftJoinCondition) where TModelLeft : class, new() where TModelRight : class, new();

        /// <summary>
        /// Builds Right Join Conditions.
        /// </summary>
        /// <typeparam name="TModelLeft">Generic Poco object of the Left Table.</typeparam>
        /// <typeparam name="TModelRight">Generic Poco object of the Right Table.</typeparam>
        /// <param name="rightJoinCondition">Right Join Condition Expression.</param>
        /// <returns>Right Join Conditions in string Format.</returns>
        string BuildRightJoinConditions<TModelLeft, TModelRight>(Expression<Func<TModelLeft, TModelRight, bool>> rightJoinCondition) where TModelLeft : class, new() where TModelRight : class, new();

        /// <summary>
        /// Builds Inner Join Conditions.
        /// </summary>
        /// <typeparam name="TModelLeft">Generic Poco object of the Left Table.</typeparam>
        /// <typeparam name="TModelRight">Generic Poco object of the Right Table.</typeparam>
        /// <param name="innerJoinCondition">Inner Join Condition Expression.</param>
        /// <returns>Inner Join Conditions in string Format.</returns>
        string BuildInnerJoinConditions<TModelLeft, TModelRight>(Expression<Func<TModelLeft, TModelRight, bool>> innerJoinCondition) where TModelLeft : class, new() where TModelRight : class, new();

        /// <summary>
        /// Returns the Parameters added to the Query.
        /// </summary>
        /// <returns>Parameters added to the Query.</returns>
        List<SqlParameter> GetParameters();

        /// <summary>
        /// Builds the Update set conditions.
        /// </summary>
        /// <typeparam name="TModel">Generic POCO Model.</typeparam>
        /// <param name="model">Poco Model Object.</param>
        string BuildUpdateSetConditions<TModel>(TModel model) where TModel : class, new();

        /// <summary>
        /// Builds Insert Conditions form the Poco Model Provided.
        /// </summary>
        /// <typeparam name="TModel">Generic POCO Model.</typeparam>
        /// <param name="model">Poco Model Object.</param>
        string BuildInsertConditions<TModel>(TModel model) where TModel : class, new();
    }
}
