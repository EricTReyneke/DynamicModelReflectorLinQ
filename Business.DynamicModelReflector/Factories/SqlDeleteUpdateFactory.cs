using Business.DynamicModelReflector.Conditions;
using Business.DynamicModelReflector.Executables;
using Business.DynamicModelReflector.Interfaces;
using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Factories
{
    public class SqlDeleteUpdateFactory<TModel> : IDeleteUpdateFactory<TModel> where TModel : class, new()
    {
        #region Fields
        /// <summary>
        /// Database Context.
        /// </summary>
        IContext<TModel> _context;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs the SqlDeleteUpdateFactory and allowes for IContext Injection.
        /// </summary>
        /// <param name="context"></param>
        public SqlDeleteUpdateFactory(IContext<TModel> context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public IExecutable<TModel> Where(Expression<Func<TModel, bool>> whereCondition)
        {
            try
            {
                IWhere<TModel> sqlWhere = new SqlWhere<TModel>(_context);
                sqlWhere.Where(whereCondition);
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