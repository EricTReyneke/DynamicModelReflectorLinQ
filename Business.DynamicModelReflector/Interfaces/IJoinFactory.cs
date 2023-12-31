﻿using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.Enums;
using System.Linq.Expressions;
using System.Numerics;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface IJoinFactory<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Builds the columns which will be selected in query.
        /// </summary>
        /// <param name="selectCondition">columns to select.</param>
        /// <returns>ILoadJoinFactory for query manipulation.</returns>
        IJoinFactory<TModel> Select(params Expression<Func<TModel, object>>[] selectCondition);

        /// <summary>
        /// Builds Query for Left Joins.
        /// </summary>
        /// <param name="joinCondition">Left join Expression.</param>
        /// <returns>ILoadJoinFactory for query manipulation.</returns>
        IJoinFactory<TModel> LeftJoin(Expression<Func<TModel, object>> joinCondition);

        /// <summary>
        /// Builds Query for Right Joins.
        /// </summary>
        /// <param name="joinCondition">Right join Expression.</param>
        /// <returns>ILoadJoinFactory for query manipulation.</returns>
        IJoinFactory<TModel> RightJoin(Expression<Func<TModel, object>> joinCondition);

        /// <summary>
        /// Builds Query for Inner Joins.
        /// </summary>
        /// <param name="joinCondition">Inner join Expression.</param>
        /// <returns>ILoadJoinFactory for query manipulation.</returns>
        IJoinFactory<TModel> InnerJoin(Expression<Func<TModel, object>> joinCondition);

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