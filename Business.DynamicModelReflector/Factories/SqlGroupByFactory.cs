using Business.DynamicModelReflector.Conditions;
using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.Enums;
using Business.DynamicModelReflector.Executables;
using Business.DynamicModelReflector.Interfaces;
using System.Linq.Expressions;
using System.Text;

namespace Business.DynamicModelReflector.Factories
{
    public class SqlGroupByFactory<TModel> : IGroupByFactory<TModel> where TModel : class, new()
    {
        #region Fields
        /// <summary>
        /// Database Context.
        /// </summary>
        IContext<TModel> _context;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs the SqlGroupByFactory and allowes for IContext Injection.
        /// </summary>
        /// <param name="context"></param>
        public SqlGroupByFactory(IContext<TModel> context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public IExecutable<TModel> OrderBy(params (Expression<Func<TModel, object>> orderByProperty, SqlOrderByMenu orderByMenu)[] orderByConditions)
        {
            try
            {
                IOrderBy<TModel> sqlOrderBy = new SqlOrderBy<TModel>(_context);
                sqlOrderBy.OrderBy(orderByConditions);
                return new SqlExecutable<TModel>(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public void Execute()
        {
            try
            {
                IExecutable<TModel> sqlExecutable = new SqlExecutable<TModel>(_context);
                sqlExecutable.Execute();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }
        #endregion
    }
}