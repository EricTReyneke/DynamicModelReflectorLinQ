using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.Executables;
using Business.DynamicModelReflector.Interfaces;
using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Conditions
{
    public class SqlOrderBy<TModel> : IOrderBy<TModel> where TModel : class, new()
    {
        #region Fields
        /// <summary>
        /// Database Context.
        /// </summary>
        IContext<TModel> _context;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs the SqlOrderBy and allowes for IContext Injection.
        /// </summary>
        /// <param name="context"></param>
        public SqlOrderBy(IContext<TModel> context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public void OrderBy(params (Expression<Func<TModel, object>> orderByProperty, OrderByMenu orderByMenu)[] orderByConditions)
        {
            try
            {
                _context.StringBuilder.Append(_context.QueryBuilder.BuildOrderByConditions(orderByConditions));
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}