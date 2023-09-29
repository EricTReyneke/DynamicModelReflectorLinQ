using Business.DynamicModelReflector.Interfaces;
using Newtonsoft.Json;
using System.Data;

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
        public void Execute()
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
        /// <exception cref="Exception"></exception>
        private void ExecuteInsertQuery()
        {
            if (_context.Models == null)
            {
                _context.DataOperations.InsertTableData(_context.StringBuilder.ToString(), _context.QueryBuilder);
                return;
            }

            string[] insertQueries = _context.StringBuilder.ToString().Split("\n");

            if (insertQueries.Length != _context.Models.Count())
                throw new Exception("The amount of queries do not corrosond to the number of Models.");

            List<TModel> models = _context.Models as List<TModel> ?? _context.Models.ToList();

            for (int i = 0; i < models.Count; i++)
                _context.DataOperations.InsertTableData(insertQueries[i], _context.QueryBuilder);
        }


        /// <summary>
        /// Maps Properties to the POCO model with the data received from the database.
        /// </summary>
        /// <param name="tableData">DataTable which holds the data received from the Database.</param>
        /// <param name="setDataModel">Poco Model which the data will be inserted.</param>
        private void MapProperties(DataTable tableData, TModel setDataModel)
        {
            if(tableData.Rows.Count > 0)
                setDataModel = JsonConvert.DeserializeObject<TModel>(JsonConvert.SerializeObject(tableData.Rows[0]));
        }

        /// <summary>
        /// Maps Properties to the POCO models with the data received from the database.
        /// </summary>
        /// <param name="tableData">DataTable which holds the data received from the Database.</param>
        /// <param name="dataModels">ICollection of Data Models where the data will be inserted.</param>
        private void MapProperties(DataTable tableData, ICollection<TModel> dataModels)
        {
            foreach (var dataRow in JsonConvert.DeserializeObject<List<TModel>>(JsonConvert.SerializeObject(tableData)))
                dataModels.Add(dataRow);
        }
        #endregion
    }
}