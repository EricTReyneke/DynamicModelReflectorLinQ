using Business.DynamicModelReflector.Interfaces;
using System.Linq.Expressions;

namespace Business.DynamicModelReflector.Conditions
{
    public class SqlJoin<TModel> : IJoin<TModel> where TModel : class, new()
    {
        #region Fields
        /// <summary>
        /// Database Context.
        /// </summary>
        IContext<TModel> _context;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs the SqlJoin and allowes for IContext Injection.
        /// </summary>
        /// <param name="context"></param>
        public SqlJoin(IContext<TModel> context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public void LeftJoin(Expression<Func<TModel, object>> joinCondition)
        {
            try
            {
                PrepareJoinCondition();
                _context.StringBuilder.Append(AddJoinTableToContext(_context.QueryBuilder.BuildLeftJoinConditions(joinCondition)));
            }
            catch
            {
                throw;
            }
        }

        public void RightJoin(Expression<Func<TModel, object>> joinCondition)
        {
            try
            {
                PrepareJoinCondition();
                _context.StringBuilder.Append(AddJoinTableToContext(_context.QueryBuilder.BuildRightJoinConditions(joinCondition)));
            }
            catch
            {
                throw;
            }
        }

        public void InnerJoin(Expression<Func<TModel, object>> joinCondition)
        {
            try
            {
                PrepareJoinCondition();
                _context.StringBuilder.Append(AddJoinTableToContext(_context.QueryBuilder.BuildInnerJoinConditions(joinCondition)));
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Prepares the StringBuilder for the join functionality to be added.
        /// </summary>
        private void PrepareJoinCondition()
        {
            string selectedColumns = _context.StringBuilder.ToString().Split(" \nFrom")[0];
            _context.StringBuilder.Clear().Append($"{selectedColumns},");
        }

        /// <summary>
        /// Addes the Joined table to the JoinedTables ICollection Property in the IContext and returns the Query.
        /// </summary>
        /// <param name="joinContext">Stores the JoinedTable in the Key and the Query in the Value of the KeyValuePairs.</param>
        /// <returns>Join Query String.</returns>
        private string AddJoinTableToContext(KeyValuePair<string, string> joinContext)
        {
            _context.JoinedTables.Add(joinContext.Key);
            return joinContext.Value;
        }
        #endregion
    }
}