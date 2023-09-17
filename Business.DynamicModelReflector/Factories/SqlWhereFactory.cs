using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.Executables;
using Business.DynamicModelReflector.Interfaces;
using System.Linq.Expressions;
using System.Text;

namespace Business.DynamicModelReflector.Factories
{
    public class SqlWhereFactory<TModel> : IWhereFactory<TModel> where TModel : class, new()
    {
        #region Fields
        /// <summary>
        /// Database Context.
        /// </summary>
        IContext<TModel> _context;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs the SqlWhereFactory and allowes for IContext Injection.
        /// </summary>
        /// <param name="context"></param>
        public SqlWhereFactory(IContext<TModel> context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public IGroupByFactory<TModel> GroupBy(Expression<Func<TModel, object>> groupByCondition)
        {
            try
            {
                return new SqlGroupByFactory<TModel>(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public IExecutable<TModel> OrderBy(Expression<Func<TModel, OrderByMenu>> orderByCondition)
        {
            try
            {

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