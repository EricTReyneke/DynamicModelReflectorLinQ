﻿using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.Enums;
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
        /// <returns>String of the Left Join Conditions.</returns>
        string BuildLeftJoinConditions<TModel>(Expression<Func<TModel, object>> joinCondition) where TModel : class, new();

        /// <summary>
        /// Builds the Right Join query conditions.
        /// </summary>
        /// <typeparam name="TModel">Generic POCO</typeparam>
        /// <param name="joinCondition">Join Expression</param>
        /// <returns>String of the Right Join Conditions.</returns>
        string BuildRightJoinConditions<TModel>(Expression<Func<TModel, object>> joinCondition) where TModel : class, new();

        /// <summary>
        /// Builds the Inner Join query conditions.
        /// </summary>
        /// <typeparam name="TModel">Generic POCO</typeparam>
        /// <param name="joinCondition">Join Expression</param>
        /// <returns>String of the Inner Join Conditions.</returns>
        string BuildInnerJoinConditions<TModel>(Expression<Func<TModel, object>> joinCondition) where TModel : class, new();

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
        /// <param name="idOffset">Offset to be applied to the ID, if applicable.</param>
        string BuildInsertConditions<TModel>(TModel model, int idOffset) where TModel : class, new();

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
