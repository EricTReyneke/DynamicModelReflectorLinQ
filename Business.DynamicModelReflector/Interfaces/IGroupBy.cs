using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.Enums;
using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface IGroupBy<TModel> where TModel : class, new()
    {
        /// <summary>
        /// Builds GroupBy Conditions.
        /// </summary>
        /// <param name="groupByCondition">GroupBy Expression.</param>
        void GroupBy(params (Expression<Func<TModel, object>> groupByProperty, SqlAggregateFunctionMenu aggregateFunctionMenu)[] groupByCondition);
    }
}