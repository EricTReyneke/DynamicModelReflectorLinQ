﻿using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.Interfaces;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace Business.DynamicModelReflector.QueryBuilders
{
    public class SqlQueryBuilder : ExpressionVisitor, IQueryBuilder
    {
        #region Fields
        /// <summary>
        /// Condition StringBuilder.
        /// </summary>
        private StringBuilder _condition = new();

        /// <summary>
        /// Parameters of the conditions added to the query.
        /// </summary>
        private List<SqlParameter> _parameters = new();

        /// <summary>
        /// Last used property name which will be added as a parameter to the query.
        /// </summary>
        private string _propertyParameterName = string.Empty;
        #endregion

        #region Public Methods
        public string BuildSelectConditions<TModel>(params Expression<Func<TModel, object>>[] selectCondition)
        {
            StringBuilder selectColumnBuilder = new($"Select");

            foreach (Expression<Func<TModel, object>> columns in selectCondition)
            {
                Expression body = columns.Body is UnaryExpression unary ? unary.Operand : columns.Body;

                if (body is MemberExpression member)
                    selectColumnBuilder.Append($" {member.Member.Name},");
            }

            return selectColumnBuilder.ToString().TrimEnd(',') + $" \nFrom {typeof(TModel).Name}";
        }

        public string BuildWhereConditions<TModel>(Expression<Func<TModel, bool>> whereCondition)
        {
            try
            {
                _condition.Clear();
                _parameters.Clear();
                _propertyParameterName = string.Empty;
                _condition.AppendLine("\n");
                Visit(whereCondition);
                return _condition.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Message: {ex.Message}\nInner Exception: {ex.InnerException}\nStack Trace: {ex.StackTrace}");
                throw;
            }
        }

        public string BuildLeftJoinConditions<TModelLeft, TModelRight>(Expression<Func<TModelLeft, TModelRight, bool>> leftJoinCondition)
            where TModelLeft : class, new() where TModelRight : class, new() =>
                $" \nLeft Join {typeof(TModelRight).Name} On {typeof(TModelLeft).Name}.{((MemberExpression)((BinaryExpression)leftJoinCondition.Body).Left).Member.Name}" +
                    $" = {typeof(TModelRight).Name}.{((MemberExpression)((BinaryExpression)leftJoinCondition.Body).Right).Member.Name}";

        public string BuildRightJoinConditions<TModelLeft, TModelRight>(Expression<Func<TModelLeft, TModelRight, bool>> rightJoinCondition)
            where TModelLeft : class, new() where TModelRight : class, new() =>
                $" \nRight Join {typeof(TModelRight).Name} On {typeof(TModelLeft).Name}.{((MemberExpression)((BinaryExpression)rightJoinCondition.Body).Left).Member.Name}" +
                    $" = {typeof(TModelRight).Name}.{((MemberExpression)((BinaryExpression)rightJoinCondition.Body).Right).Member.Name}";

        public string BuildInnerJoinConditions<TModelLeft, TModelRight>(Expression<Func<TModelLeft, TModelRight, bool>> innerJoinCondition)
            where TModelLeft : class, new() where TModelRight : class, new() =>
                $" \nInner Join {typeof(TModelRight).Name} On {typeof(TModelLeft).Name}.{((MemberExpression)((BinaryExpression)innerJoinCondition.Body).Left).Member.Name}" +
                    $" = {typeof(TModelRight).Name}.{((MemberExpression)((BinaryExpression)innerJoinCondition.Body).Right).Member.Name}";

        public void BuildGroupByConditions<TModel>(StringBuilder queryStatment, params (Expression<Func<TModel, object>> groupByProperty, AggregateFunctionMenu aggregateFunctionMenu)[] groupByCondition) where TModel : class, new()
        {
            string query = queryStatment.ToString();
            Match match = new Regex(@"Select\s+(.*?)\s+From", RegexOptions.IgnoreCase | RegexOptions.Compiled).Match(query);

            if (!match.Success)
                throw new Exception("Query was not in the correct format: " + query);

            string[] columnArray = match.Groups[1].Value.Split(',');

            string groupByClause = " \nGroup By " + string.Join(", ", columnArray);

            foreach ((Expression<Func<TModel, object>> groupByProperty, AggregateFunctionMenu aggregateFunctionMenu) groupBy in groupByCondition)
            {
                Expression body = groupBy.groupByProperty.Body;
                if (body is UnaryExpression unary)
                    body = unary.Operand;

                if (body is MemberExpression member)
                    query = query.Replace(member.Member.Name, $"{groupBy.aggregateFunctionMenu}({member.Member.Name}) As {member.Member.Name}");
            }

            queryStatment.Clear();
            queryStatment.Append(query);
            queryStatment.Append(groupByClause);
        }


        public string BuildOrderByConditions<TModel>(params (Expression<Func<TModel, object>> orderByProperty, OrderByMenu orderByMenu)[] orderByCondition) where TModel : class, new()
        {
            StringBuilder orderByConditionBuilder = new(" \nOrder By");

            foreach ((Expression<Func<TModel, object>> orderByProperty, OrderByMenu orderByMenu) orderBy in orderByCondition)
            {
                Expression body = orderBy.orderByProperty.Body is UnaryExpression unary ? unary.Operand : orderBy.orderByProperty.Body;

                if (body is MemberExpression member)
                    orderByConditionBuilder.Append($" {member.Member.Name} {orderBy.orderByMenu},");
            }

            return orderByConditionBuilder.ToString().TrimEnd(',');
        }

        public string BuildUpdateSetConditions<TModel>(TModel model) where TModel : class, new()
        {
            StringBuilder stringBuilder = new(" \nSet");

            PropertyInfo[] propertyInfos = typeof(TModel).GetProperties();

            for (int i = 0; i < propertyInfos.Length; i++)
            {
                GenerateSqlParameter(propertyInfos[i], model);

                stringBuilder.Append($" {propertyInfos[i].Name} = @{propertyInfos[i].Name}{_parameters.Count - 1},");
            }

            return stringBuilder.ToString().TrimEnd(',');
        }

        public string BuildInsertConditions<TModel>(TModel model) where TModel : class, new()
        {
            StringBuilder stringBuilder = new(" \n(");

            PropertyInfo[] propertyInfos = typeof(TModel).GetProperties();

            for (int i = 0; i < propertyInfos.Length; i++)
            {
                stringBuilder.Append($" {propertyInfos[i].Name}");

                if (i != propertyInfos.Length - 1)
                    stringBuilder.Append(',');
                else
                    stringBuilder.Append(") \nValues (");
            }

            for (int i = 0; i < propertyInfos.Length; i++)
            {
                GenerateSqlParameter(propertyInfos[i], model);

                stringBuilder.Append($" @{propertyInfos[i].Name}{_parameters.Count - 1}");

                if (i != propertyInfos.Length - 1)
                    stringBuilder.Append(',');
            }

            stringBuilder.Append(')');

            return stringBuilder.ToString();
        }

        public List<SqlParameter> GetParameters() =>
            _parameters;
        #endregion

        #region Protected Methods
        protected override Expression VisitBinary(BinaryExpression node)
        {
            Visit(node.Left);

            switch (node.NodeType)
            {
                case ExpressionType.AndAlso:
                    _condition.Append(" AND ");
                    break;
                case ExpressionType.OrElse:
                    _condition.Append(" OR ");
                    break;
                case ExpressionType.Equal:
                    _condition.Append(" = ");
                    break;
                case ExpressionType.NotEqual:
                    _condition.Append(" <> ");
                    break;
                case ExpressionType.LessThan:
                    _condition.Append(" < ");
                    break;
                case ExpressionType.LessThanOrEqual:
                    _condition.Append(" <= ");
                    break;
                case ExpressionType.GreaterThan:
                    _condition.Append(" > ");
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    _condition.Append(" >= ");
                    break;
                default:
                    throw new NotSupportedException($"Binary operator '{node.NodeType}' is not supported.");
            }

            Visit(node.Right);

            return node;
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            _condition.Append(node.Member.Name);
            _propertyParameterName = node.Member.Name;
            return base.VisitMember(node);
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            string paramName = $"@{_propertyParameterName}{_parameters.Count}";
            _condition.Append(paramName);

            if (node.Type == typeof(string) || node.Type == typeof(DateTime))
                _parameters.Add(new SqlParameter(paramName, Convert.ToString(node.Value)));
            else
                _parameters.Add(new SqlParameter(paramName, node.Value));

            return base.VisitConstant(node);
        }

        /// <summary>
        /// Generates SqlParameters and adds them to a list of SqlParameters.
        /// </summary>
        /// <typeparam name="TModel">Generic Poco.</typeparam>
        /// <param name="propertyInfo">Specified Property info.</param>
        /// <param name="model">Poco object.</param>
        private void GenerateSqlParameter<TModel>(PropertyInfo propertyInfo, TModel model) where TModel : class, new()
        {
            object value = propertyInfo.GetValue(model) ?? DBNull.Value;

            if (propertyInfo.PropertyType == typeof(string) ||
                propertyInfo.PropertyType == typeof(DateTime) ||
                Nullable.GetUnderlyingType(propertyInfo.PropertyType) == typeof(DateTime) ||
                Nullable.GetUnderlyingType(propertyInfo.PropertyType) == typeof(string))
            {
                _parameters.Add(new SqlParameter($"@{propertyInfo.Name}{_parameters.Count}", value));
                return;
            }

            _parameters.Add(new SqlParameter($"@{propertyInfo.Name}{_parameters.Count}", value));
        }

        #endregion
    }
}