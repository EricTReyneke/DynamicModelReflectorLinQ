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
