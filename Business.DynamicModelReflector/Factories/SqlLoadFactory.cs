using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.Executables;
using Business.DynamicModelReflector.Interfaces;
using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Factories
{
    public class SqlLoadFactory<TModel> : ILoadFactory<TModel> where TModel : class, new()
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
        public SqlLoadFactory(IContext<TModel> context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public IJoinFactory<TModel> Select(params Expression<Func<TModel, object>>[] selectCondition)
        {
            try
            {
                _context.StringBuilder.Clear();
                _context.StringBuilder.Append(_context.QueryBuilder.BuildSelectConditions(selectCondition));
                return new SqlJoinFactory<TModel>(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public IJoinFactory<TModel> LeftJoin<TModelLeft, TModelRight>(Expression<Func<TModelLeft, TModelRight, bool>> joinCondition) where TModelLeft : class, new() where TModelRight : class, new()
        {
            try
            {
                _context.StringBuilder.Append(_context.QueryBuilder.BuildLeftJoinConditions(joinCondition));
                return new SqlJoinFactory<TModel>(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public IJoinFactory<TModel> RightJoin<TModelLeft, TModelRight>(Expression<Func<TModelLeft, TModelRight, bool>> joinCondition) where TModelLeft : class, new() where TModelRight : class, new()
        {
            try
            {
                _context.StringBuilder.Append(_context.QueryBuilder.BuildRightJoinConditions(joinCondition));
                return new SqlJoinFactory<TModel>(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public IJoinFactory<TModel> InnerJoin<TModelLeft, TModelRight>(Expression<Func<TModelLeft, TModelRight, bool>> joinCondition) where TModelLeft : class, new() where TModelRight : class, new()
        {
            try
            {
                _context.StringBuilder.Append(_context.QueryBuilder.BuildInnerJoinConditions(joinCondition));
                return new SqlJoinFactory<TModel>(_context);
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

        public IGroupByFactory<TModel> GroupBy(params (Expression<Func<TModel, object>> groupByProperty, AggregateFunctionMenu aggregateFunctionMenu)[] groupByCondition)
        {
            try
            {
                _context.QueryBuilder.BuildGroupByConditions(_context.StringBuilder, groupByCondition);
                return new SqlGroupByFactory<TModel>(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public IExecutable<TModel> OrderBy(params (Expression<Func<TModel, object>> orderByProperty, OrderByMenu orderByMenu)[] orderByConditions)
        {
            try
            {
                _context.StringBuilder.Append(_context.QueryBuilder.BuildOrderByConditions(orderByConditions));
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
