using Business.DynamicModelReflector.Data.Model;
using Business.DynamicModelReflector.Enums;
using Business.DynamicModelReflector.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;

namespace Business.DynamicModelReflector.QueryBuilders
{
    public class SqlQueryBuilder : ExpressionVisitor, IQueryBuilder
    {
        #region Fields
        /// <summary>
        /// Condition StringBuilder.
        /// </summary>
        StringBuilder _condition = new();

        /// <summary>
        /// Parameters of the conditions added to the query.
        /// </summary>
        List<SqlParameter> _parameters = new();

        /// <summary>
        /// Last used property name which will be added as a parameter to the query.
        /// </summary>
        string _propertyParameterName = string.Empty;

        /// <summary>
        /// Generic Poco Model for where conditions.
        /// </summary>
        string _pocoModelName = string.Empty;

        /// <summary>
        /// When variables are pushed through the expressions it will be caught in variableName.
        /// </summary>
        string _variableName = string.Empty;
        #endregion

        #region Public Methods
        public string AddAllColumnsIntoSelect<TModel>() where TModel : class, new()
        {
            try
            {
                return AddAllTableColumns<TModel>();
            }
            catch
            {
                throw;
            }
        }

        public string BuildSelectConditions<TModel>(params Expression<Func<TModel, object>>[] selectCondition)
        {
            try
            {
                string modelName = typeof(TModel).Name;
                StringBuilder selectColumnBuilder = new($"Select");

                foreach (Expression<Func<TModel, object>> columns in selectCondition)
                {
                    Expression body = columns.Body is UnaryExpression unary ? unary.Operand : columns.Body;

                    if (body is MemberExpression member)
                        selectColumnBuilder.Append($" {modelName}.{member.Member.Name},");
                }

                return selectColumnBuilder.ToString().TrimEnd(',') + $" \nFrom {typeof(TModel).Name}";
            }
            catch
            {
                throw;
            }
        }

        public string BuildWhereConditions<TModel>(Expression<Func<TModel, bool>> whereCondition) where TModel : class, new()
        {
            try
            {
                _condition.Clear();
                _parameters.Clear();
                _propertyParameterName = string.Empty;
                TModel model = new();
                _pocoModelName = model.GetType().Name;
                Visit(whereCondition);
                return _condition.ToString();
            }
            catch
            {
                throw;
            }
        }

        public string BuildLeftJoinConditions<TModel>(Expression<Func<TModel, object>> joinCondition) where TModel : class, new() =>
            BuildJoinConditions("Left", joinCondition);

        public string BuildRightJoinConditions<TModel>(Expression<Func<TModel, object>> joinCondition) where TModel : class, new() =>
            BuildJoinConditions("Right", joinCondition);

        public string BuildInnerJoinConditions<TModel>(Expression<Func<TModel, object>> joinCondition) where TModel : class, new() =>
            BuildJoinConditions("Inner", joinCondition);

        public void BuildGroupByConditions<TModel>(StringBuilder queryStatment, params (Expression<Func<TModel, object>> groupByProperty, SqlAggregateFunctionMenu aggregateFunctionMenu)[] groupByCondition) where TModel : class, new()
        {
            try
            {
                string query = queryStatment.ToString();
                Match match = new Regex(@"Select\s+(.*?)\s+From", RegexOptions.IgnoreCase | RegexOptions.Compiled).Match(query);

                if (!match.Success)
                    throw new Exception("Query was not in the correct format: " + query);

                string[] columnArray = match.Groups[1].Value.Split(',');

                string groupByClause = " \nGroup By " + string.Join(", ", columnArray);

                foreach ((Expression<Func<TModel, object>> groupByProperty, SqlAggregateFunctionMenu aggregateFunctionMenu) groupBy in groupByCondition)
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
            catch
            {
                throw;
            }
        }

        public string BuildOrderByConditions<TModel>(params (Expression<Func<TModel, object>> orderByProperty, SqlOrderByMenu orderByMenu)[] orderByCondition) where TModel : class, new()
        {
            try
            {
                StringBuilder orderByConditionBuilder = new(" \nOrder By");

                foreach ((Expression<Func<TModel, object>> orderByProperty, SqlOrderByMenu orderByMenu) orderBy in orderByCondition)
                {
                    Expression body = orderBy.orderByProperty.Body is UnaryExpression unary ? unary.Operand : orderBy.orderByProperty.Body;

                    if (body is MemberExpression member)
                        orderByConditionBuilder.Append($" {typeof(TModel).Name}.{member.Member.Name} {orderBy.orderByMenu},");
                }

                return orderByConditionBuilder.ToString().TrimEnd(',');
            }
            catch
            {
                throw;
            }
        }

        public string BuildUpdateSetConditions<TModel>(TModel model) where TModel : class, new()
        {
            try
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
            catch
            {
                throw;
            }
        }

        public string BuildInsertConditions<TModel>(TModel model) where TModel : class, new()
        {
            try
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
            catch
            {
                throw;
            }
        }

        public List<SqlParameter> GetParameters() =>
            _parameters;
        #endregion

        #region Protected Methods
        protected override Expression VisitBinary(BinaryExpression node)
        {
            try
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
            catch
            {
                throw;
            }
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            try
            {
                if (!(node.Expression is ConstantExpression constExpr) || constExpr.Value == null || !constExpr.Value.GetType().Name.Contains("DisplayClass"))
                {
                    _condition.Append($"{_pocoModelName}.{node.Member.Name}");
                    _propertyParameterName = node.Member.Name;
                    return base.VisitMember(node);
                }
                _variableName = node?.ToString().Split('.').LastOrDefault();
                return base.VisitMember(node);
            }
            catch
            {
                throw;
            }
        }

        protected override Expression VisitConstant(ConstantExpression node)
        {
            try
            {
                object valueToUse;
                string paramName = $"@{_propertyParameterName}{_parameters.Count}";

                if (node.Value != null && node.Value.GetType().Name.Contains("DisplayClass"))
                    valueToUse = GetClosureVariableValue(node.Value, _variableName);
                else
                    valueToUse = node.Value;

                _condition.Append(paramName);

                _parameters.Add(valueToUse is string || valueToUse is DateTime
                    ? new SqlParameter(paramName, Convert.ToString(valueToUse))
                    : new SqlParameter(paramName, valueToUse));

                return base.VisitConstant(node);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Retrieves the value of a specified variable from a given object using reflection.
        /// </summary>
        /// <param name="nodeValue">The object from which to extract the variable's value.</param>
        /// <param name="variableName">The name of the variable whose value is to be retrieved.</param>
        /// <returns>The value of the specified variable. Returns null if the variable is not found.</returns>
        /// <remarks>
        /// This method uses reflection to access private and public fields of the object.
        /// It is intended for scenarios where accessing internal object state is necessary, such as debugging or logging. Caution is advised due to potential performance impacts and breaking of encapsulation principles.
        /// </remarks>
        private object GetClosureVariableValue(object nodeValue, string variableName) =>
                 nodeValue.GetType()
                .GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
                .Where(field => field.Name == variableName)
                .Select(field => field.GetValue(nodeValue))
                .FirstOrDefault();

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

        /// <summary>
        /// Adds all the columns from the foreign key table into the select statement.
        /// </summary>
        /// <typeparam name="TModel">Generic POCO.</typeparam>
        /// <param name="foreignTableName">Foreign Table Name.</param>
        /// <param name="model">Poco model object.</param>
        /// <returns>Adds all the columns from the foreign key table as a string.</returns>
        private string AddAllForeignKeyTableColumns<TModel>(string foreignTableName, TModel model)
        {
            Type foreignKeyTableModel = GetForeignKeyTableModel(foreignTableName, model);
            if (foreignKeyTableModel == null)
                return string.Empty;

            return AddAllTableColumns<TModel>(foreignKeyTableModel);
        }

        /// <summary>
        /// Retrieves the type of the foreign key table model from the provided model.
        /// </summary>
        /// <typeparam name="TModel">Generic POCO.</typeparam>
        /// <param name="foreignTableName">Foreign Table Name.</param>
        /// <param name="model">Poco model object.</param>
        /// <returns>Type of the foreign key table model or null if not found.</returns>
        private Type GetForeignKeyTableModel<TModel>(string foreignTableName, TModel model)
        {
            PropertyInfo property = model.GetType().GetProperty(foreignTableName);

            if (property != null)
                return property.PropertyType;

            throw new Exception($"Property {property.Name} is not a Forgein table.");
        }

        /// <summary>
        /// Adds all the table columns to Select Query.
        /// </summary>
        /// <typeparam name="TModel">Generic POCO.</typeparam>
        /// <param name="modelType">Optional specific type of the model. If not provided, uses TModel.</param>
        /// <returns>String of all properties in the model as table Columns.</returns>
        private string AddAllTableColumns<TModel>(Type modelType = null)
        {
            modelType ??= typeof(TModel);

            StringBuilder stringBuilder = new();
            string modelName = modelType.Name;

            foreach (PropertyInfo propertyInfo in modelType.GetProperties())
            {
                if (modelType.GetProperty(propertyInfo.Name).IsDefined(typeof(IgnoreDataMemberAttribute), false))
                    continue;

                stringBuilder.Append($" {modelName}.{propertyInfo.Name},");
            }

            return stringBuilder.ToString().TrimEnd(',');
        }

        /// <summary>
        /// Builds the join conditions for Select Statments.
        /// </summary>
        /// <typeparam name="TModel">Generic POCO</typeparam>
        /// <param name="joinType">The type of join statment.</param>
        /// <param name="joinCondition">Join Expression.</param>
        /// <returns>String of the Join conditions.</returns>
        /// <exception cref="Exception">Throws Exeption when the Expression is not in the correct format.</exception>
        private string BuildJoinConditions<TModel>(string joinType, Expression<Func<TModel, object>> joinCondition) where TModel : class, new()
        {
            try
            {
                string foreignKeyName = RetirevesTheForeignKeyColumnName(joinCondition);
                string? relationshipTableName = RetrieveForeignKeyTableName(foreignKeyName, new TModel());

                if (relationshipTableName == null)
                    throw new Exception("Join property is not a forgein key.");

                return $"{AddAllForeignKeyTableColumns(relationshipTableName, new TModel())} \nFrom {typeof(TModel).Name} \n{joinType} Join {relationshipTableName} On {typeof(TModel).Name}.{foreignKeyName} = {relationshipTableName}.{foreignKeyName}";
            }
            catch
            {
                throw;
            }

            throw new Exception("Expression is in the wrong format.");
        }

        /// <summary>
        /// Retrieves the Forgein Key Column Name.
        /// </summary>
        /// <typeparam name="TModel">Generic POCO</typeparam>
        /// <param name="joinCondition">Join Expression.</param>
        /// <returns>Forgein Key Column Name.</returns>
        /// <exception cref="Exception">Throws Exeption when the Expression is not in the correct format.</exception>
        private string RetirevesTheForeignKeyColumnName<TModel>(Expression<Func<TModel, object>> joinCondition)
            where TModel : class, new() =>
                joinCondition.Body is UnaryExpression unary && unary.NodeType == ExpressionType.Convert ?
                    unary.Operand.ToString().Split('.').Last()
                        : throw new Exception("Expression is in the wrong format.");
        /// <summary>
        /// Retrieves the Forgein table name from the Forgein keys attribute in the POCO Model.
        /// </summary>
        /// <typeparam name="TModel">Generic POCO</typeparam>
        /// <param name="foreignKeyName">Forgein key column/property name.</param>
        /// <param name="model">POCO object.</param>
        /// <returns>Forgein table name from the Forgein key property attribute.</returns>
        private string? RetrieveForeignKeyTableName<TModel>(string foreignKeyName, TModel model)
            where TModel : class, new() =>
                (model.GetType().GetProperty(foreignKeyName)
                    ?.GetCustomAttribute<ForeignKeyAttribute>() as ForeignKeyAttribute)?.Name;
        #endregion
    }
}