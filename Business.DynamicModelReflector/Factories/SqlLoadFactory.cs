using Business.DynamicModelReflector.Conditions;
using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.Executables;
using Business.DynamicModelReflector.Interfaces;
using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Factories
{
    public class SqlLoadFactory<TModel> : IJoinFactory<TModel> where TModel : class, new()
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
                SqlSelect<TModel> sqlSelect = new(_context);
                sqlSelect.Select(selectCondition);
                return new SqlLoadFactory<TModel>(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public IJoinFactory<TModel> LeftJoin(Expression<Func<TModel, object>> joinCondition)
        {
            try
            {
                SqlJoin<TModel> sqlJoin = new(_context);
                sqlJoin.LeftJoin(joinCondition);
                return new SqlLoadFactory<TModel>(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public IJoinFactory<TModel> RightJoin(Expression<Func<TModel, object>> joinCondition)
        {
            try
            {
                SqlJoin<TModel> sqlJoin = new(_context);
                sqlJoin.RightJoin(joinCondition);
                return new SqlLoadFactory<TModel>(_context);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public IJoinFactory<TModel> InnerJoin(Expression<Func<TModel, object>> joinCondition)
        {
            try
            {
                SqlJoin<TModel> sqlJoin = new(_context);
                sqlJoin.InnerJoin(joinCondition);
                return new SqlLoadFactory<TModel>(_context);
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
                SqlWhere<TModel> sqlWhere = new(_context);
                sqlWhere.Where(whereCondition);
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
                SqlGroupBy<TModel> sqlGroupBy = new(_context);
                sqlGroupBy.GroupBy(groupByCondition);
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
                SqlOrderBy<TModel> sqlOrderBy = new(_context);
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