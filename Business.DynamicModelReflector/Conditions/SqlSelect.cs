using Business.DynamicModelReflector.Interfaces;
using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Conditions
{
    public class SqlSelect<TModel> : ISelect<TModel> where TModel : class, new()
    {
        #region Fields
        /// <summary>
        /// Database Context.
        /// </summary>
        IContext<TModel> _context;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs the SqlSelect and allowes for IContext Injection.
        /// </summary>
        /// <param name="context"></param>
        public SqlSelect(IContext<TModel> context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public void Select(params Expression<Func<TModel, object>>[] selectCondition)
        {
            try
            {
                _context.StringBuilder.Clear();
                _context.StringBuilder.Append(_context.QueryBuilder.BuildSelectConditions(selectCondition));
            }
            catch
            {
                throw;
            }
        }
        #endregion
    }
}