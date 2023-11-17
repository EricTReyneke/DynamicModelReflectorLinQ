using Business.DynamicModelReflector.Interfaces;
using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Conditions
{
    public class SqlWhere<TModel> : IWhere<TModel> where TModel : class, new()
    {
        #region Fields
        /// <summary>
        /// Database Context.
        /// </summary>
        IContext<TModel> _context;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs the SqlWhere and allowes for IContext Injection.
        /// </summary>
        /// <param name="context"></param>
        public SqlWhere(IContext<TModel> context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public void Where(Expression<Func<TModel, bool>> whereCondition)
        {
            try
            {
                _context.StringBuilder.Append($" \nWhere {_context.QueryBuilder.BuildWhereConditions(whereCondition)}");
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}