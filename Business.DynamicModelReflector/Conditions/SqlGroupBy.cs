using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.Interfaces;
using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Conditions
{
    public class SqlGroupBy<TModel> : IGroupBy<TModel> where TModel : class, new()
    {
        #region Fields
        /// <summary>
        /// Database Context.
        /// </summary>
        IContext<TModel> _context;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs the SqlGroupBy and allowes for IContext Injection.
        /// </summary>
        /// <param name="context"></param>
        public SqlGroupBy(IContext<TModel> context)
        {
            _context = context;
        }
        #endregion

        #region
        public void GroupBy(params (Expression<Func<TModel, object>> groupByProperty, AggregateFunctionMenu aggregateFunctionMenu)[] groupByCondition)
        {
            try
            {
                _context.QueryBuilder.BuildGroupByConditions(_context.StringBuilder, groupByCondition);
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}