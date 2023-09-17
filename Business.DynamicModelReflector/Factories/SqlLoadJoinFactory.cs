using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.Executables;
using Business.DynamicModelReflector.Interfaces;
using System.Linq.Expressions;
using System.Text;

namespace Business.DynamicModelReflector.Factories
{
    public class SqlLoadJoinFactory<TModel> : ILoadJoinFactory<TModel> where TModel : class, new()
    {
        #region Fields
        /// <summary>
        /// Database Context.
        /// </summary>
        IContext<TModel> _context;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs the SqlLoadJoinFactory and allowes for IContext Injection.
        /// </summary>
        /// <param name="context"></param>
        public SqlLoadJoinFactory(IContext<TModel> context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public ILoadJoinFactory<TModel> LeftJoin<TModelLeft, TModelRight>(Expression<Func<TModelLeft, TModelRight, bool>> joinCondition) where TModelLeft : class, new() where TModelRight : class, new()
        {
            try
            {
                _context.StringBuilder.Append(_context.QueryBuilder.BuildLeftJoinConditions(joinCondition));
                return new SqlLoadJoinFactory<TModel>(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public ILoadJoinFactory<TModel> RightJoin<TModelLeft, TModelRight>(Expression<Func<TModelLeft, TModelRight, bool>> joinCondition) where TModelLeft : class, new() where TModelRight : class, new()
        {
            try
            {
                _context.StringBuilder.Append(_context.QueryBuilder.BuildRightJoinConditions(joinCondition));
                return new SqlLoadJoinFactory<TModel>(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public ILoadJoinFactory<TModel> InnerJoin<TModelLeft, TModelRight>(Expression<Func<TModelLeft, TModelRight, bool>> joinCondition) where TModelLeft : class, new() where TModelRight : class, new()
        {
            try
            {
                _context.StringBuilder.Append(_context.QueryBuilder.BuildInnerJoinConditions(joinCondition));
                return new SqlLoadJoinFactory<TModel>(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public IWhereFactory<TModel> Where(Expression<Func<TModel, bool>> whereCondition)
        {
            try
            {
                _context.StringBuilder.Append($"Where {_context.QueryBuilder.BuildWhereConditions(whereCondition)}");
                return new SqlWhereFactory<TModel>(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public IGroupByFactory<TModel> OrderBy(Func<TModel, object> keySelector, OrderByMenu orderByMenu)
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

        public IExecutable<TModel> OrderBy(params (Func<TModel, object> orderByProperty, OrderByMenu orderByMenu)[] propertiesToOrderBy)
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