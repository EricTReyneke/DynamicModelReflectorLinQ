using Business.DynamicModelReflector.Interfaces;
using Business.DynamicModelReflector.Models;
using Newtonsoft.Json;
using System.Data;
using System.Reflection;
using System.Runtime.Serialization;

namespace Business.DynamicModelReflector.Executables
{
    public class SqlExecutable<TModel> : IExecutable<TModel> where TModel : class, new()
    {
        #region Fields
        /// <summary>s
        /// Database Context.
        /// </summary>
        IContext<TModel> _context;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs the SqlExecutable and allowes for IContext Injection.
        /// </summary>
        /// <param name="context"></param>
        public SqlExecutable(IContext<TModel> context)
        {
            _context = context;
        }
        #endregion

        #region Public Methods
        public ICollection<PrimaryKeyInfo> Execute()
        {
            switch (_context.StringBuilder?.ToString().Split(' ')[0].ToLower())
            {
                case "select":
                    ExecuteSelectQuery();
                    break;
                case "delete":
                    _context.DataOperations.DeleteTableData(_context.StringBuilder.ToString(), _context.QueryBuilder);
                    break;
                case "update":
                    _context.DataOperations.UpdateTableData(_context.StringBuilder.ToString(), _context.QueryBuilder);
                    break;
                case "insert":
                    ExecuteInsertQuery();
                    break;
                default:
                    throw new Exception($"The query: \"{_context.StringBuilder}\" is not supported.");
            }

            return _context.PrimaryKeyCreationTracker;
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Validates what model to use in the _context and Executes query and the correct overload method.
        /// </summary>
        private void ExecuteSelectQuery()
        {
            if (_context.Model != null)
            {
                MapProperties(_context.DataOperations.RetrieveTableData(_context.StringBuilder.ToString(), _context.QueryBuilder), _context.Model);
                return;
            }

            MapProperties(_context.DataOperations.RetrieveTableData(_context.StringBuilder.ToString(), _context.QueryBuilder), (ICollection<TModel>)_context.Models);
        }

        /// <summary>
        /// Validates what model to use and execute the query accordingly.
        /// </summary>
        /// <exception cref="Exception">The amount of queries do not corrosond to the number of Models.</exception>
        private void ExecuteInsertQuery() =>
            _context.DataOperations.BulkInsert<TModel>(_context.DataTable);


        /// <summary>
        /// Mapes Properties to the POCO model with the data recieved from the database.
        /// </summary>
        /// <param name="tableData">DataTable which holds the data recieved from the Database.</param>
        /// <param name="setDataModel">Poco Model which the data will be inserted.</param>
        private void MapProperties(DataTable tableData, TModel setDataModel)
        {
            if (tableData.Rows.Count == 0) return;

            IEnumerable<PropertyInfo> properties = typeof(TModel).GetProperties()
                .Where(PropertyInfo => PropertyInfo.CanWrite && tableData.Columns.Contains(PropertyInfo.Name))
                .ToList();

            Parallel.ForEach(properties, propertyInfo =>
            {
                object value = tableData.Rows[0][propertyInfo.Name];

                if (value == DBNull.Value)
                    value = null;
                else
                    value = TypeConversion(value.ToString(), propertyInfo.PropertyType);

                propertyInfo.SetValue(setDataModel, value);
            });
        }

        /// <summary>
        /// Maps Properties to the POCO models with the data received from the database.SSS
        /// </summary>
        /// <param name="tableData">DataTable which holds the data received from the Database.</param>
        /// <param name="dataModels">ICollection of Data Models where the data will be inserted.</param>
        private void MapProperties(DataTable tableData, ICollection<TModel> dataModels)
        {
            foreach (TModel dataRow in JsonConvert.DeserializeObject<List<TModel>>(JsonConvert.SerializeObject(tableData)))
                dataModels.Add(dataRow);
        }

        /// <summary>
        /// Converts the string value into the propertyType provided.
        /// </summary>
        /// <param name="propertyValue">Value which would be converted into the type provided.</param>
        /// <param name="propertyType">Poco Property type.</param>
        /// <returns>Object which would be the same type as the propertyType provided.</returns>
        private object TypeConversion(string propertyValue, Type propertyType)
        {
            if (string.IsNullOrEmpty(propertyValue))
                return null;

            if (Nullable.GetUnderlyingType(propertyType) != null)
                propertyType = Nullable.GetUnderlyingType(propertyType);

            if(propertyType == typeof(Guid))
                return Guid.Parse(propertyValue);

            return Convert.ChangeType(propertyValue, propertyType);
        }
        #endregion
    }
}