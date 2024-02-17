using Business.DynamicModelReflector.Enums;
using Business.DynamicModelReflector.Models;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Text;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface IQueryBuilder
    {
        /// <summary>
        /// Adds all Columns to Select Query.
        /// </summary>
        /// <typeparam name="TModel">Generic Poco.</typeparam>
        /// <returns>String of all Column Names.</returns>
        string AddAllColumnsIntoSelect<TModel>() where TModel : class, new();

        /// <summary>
        /// Builds the columns which will be selected in query.
        /// </summary>
        /// <param name="selectCondition">columns to select.</param>
        /// <returns>ILoadJoinFactory for query manipulation.</returns>
        string BuildSelectConditions<TModel>(params Expression<Func<TModel, object>>[] selectCondition);

        /// <summary>
        /// Builds Where Conditions.
        /// </summary>
        /// <typeparam name="TModel">Generic Poco Model.</typeparam>
        /// <param name="whereCondition">Where Condition Expression.</param>
        /// <returns>Where Condition String.</returns>
        string BuildWhereConditions<TModel>(Expression<Func<TModel, bool>> whereCondition) where TModel : class, new();

        /// <summary>
        /// Builds the Left Join query conditions.
        /// </summary>
        /// <typeparam name="TModel">Generic POCO</typeparam>
        /// <param name="joinCondition">Join Expression</param>
        /// <returns>KeyValuePair of the Joined table name and the Left Join Query.</returns>
        KeyValuePair<string, string> BuildLeftJoinConditions<TModel>(Expression<Func<TModel, object>> joinCondition) where TModel : class, new();

        /// <summary>
        /// Builds the Right Join query conditions.
        /// </summary>
        /// <typeparam name="TModel">Generic POCO</typeparam>
        /// <param name="joinCondition">Join Expression</param>
        /// <returns>KeyValuePair of the Joined table name and the Right Join Query.</returns>
        KeyValuePair<string, string> BuildRightJoinConditions<TModel>(Expression<Func<TModel, object>> joinCondition) where TModel : class, new();

        /// <summary>
        /// Builds the Inner Join query conditions.
        /// </summary>
        /// <typeparam name="TModel">Generic POCO</typeparam>
        /// <param name="joinCondition">Join Expression</param>
        /// <returns>KeyValuePair of the Joined table name and the Inner Join Query.</returns>
        KeyValuePair<string, string> BuildInnerJoinConditions<TModel>(Expression<Func<TModel, object>> joinCondition) where TModel : class, new();

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
        /// Creates a DataTable for bulk insertion of a collection of models.
        /// </summary>
        /// <param name="models">The collection of models to be inserted into the DataTable.</param>
        /// <typeparam name="TModel">The type of the model, must be a class with a parameterless constructor.</typeparam>
        /// <returns>A DataTable filled with the properties and values of the provided models.</returns>
        /// <remarks>
        /// This method builds a DataTable with columns corresponding to the properties of TModel. It uses reflection to retrieve property information and primary key details. The DataTable is populated by calling GenerateDataTable.
        /// </remarks>
        Dictionary<DataTable, IEnumerable<PrimaryKeyInfo>> BuildBulkInsert<TModel>(IEnumerable<TModel> models) where TModel : class, new();

        /// <summary>
        /// Build the GroupBy conditions.
        /// </summary>
        /// <typeparam name="TModel">Generic POCO.</typeparam>
        /// <param name="QueryStatment">Group by query expression.</param>
        /// <param name="groupByCondition">GroupBy Aggregate function.</param>
        void BuildGroupByConditions<TModel>(StringBuilder QueryStatment, params (Expression<Func<TModel, object>> groupByProperty, SqlAggregateFunctionMenu aggregateFunctionMenu)[] groupByCondition) where TModel : class, new();

        /// <summary>
        /// Builds Order By conditions.
        /// </summary>
        /// <typeparam name="TModel">Generic POCO.</typeparam>
        /// <param name="orderByCondition">Order By expression.</param>
        /// <returns>Order By condition.</returns>
        string BuildOrderByConditions<TModel>(params (Expression<Func<TModel, object>> orderByProperty, SqlOrderByMenu orderByMenu)[] orderByCondition) where TModel : class, new();
    }
}
