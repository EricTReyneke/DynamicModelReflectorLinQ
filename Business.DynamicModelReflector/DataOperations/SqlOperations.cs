using Business.DynamicModelReflector.Interfaces;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using Microsoft.Extensions.Configuration;

namespace Business.DynamicModelReflector.DataOperations
{
    public class SqlOperations : IDataOperations
    {
        #region Fields
        /// <summary>
        /// Connection string.
        /// </summary>
        string _connectionString;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs the SqlDataOperations and sets the connection string from the App.Config.
        /// </summary>
        public SqlOperations()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        }

        /// <summary>
        /// Constructs the SqlDataOperations and allows for IConfiguration injection to retrieve the Connection String From 
        /// The "DBConnectionString" key in the appsettings.json.
        /// </summary>
        /// <param name="configuration"></param>
        public SqlOperations(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBConnectionString");
        }
        #endregion

        #region Public Methods
        public DataTable RetrieveTableData(string selectStatment, IQueryBuilder queryBuilder)
        {
            using (SqlConnection sqlConnection = CreateConnection())
            {
                using ( SqlCommand sqlCommand = new SqlCommand(selectStatment, sqlConnection))
                {
                    List<SqlParameter> conditionalParameters = CloneConditionalParameters(queryBuilder.GetParameters());

                    if (conditionalParameters != null)
                        sqlCommand.Parameters.AddRange(conditionalParameters.ToArray());

                    try
                    {
                        using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                        {
                            DataTable dataTable = new DataTable();
                            sqlDataAdapter.Fill(dataTable);
                            return dataTable;
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }

        public void DeleteTableData(string deleteStatement, IQueryBuilder queryBuilder)
        {
            try
            {
                ExcuteQuery(deleteStatement, queryBuilder);
            }
            catch
            {
                throw;
            }
        }

        public void InsertTableData(string insertStatement, IQueryBuilder queryBuilder)
        {
            try
            {
                ExcuteQuery(insertStatement, queryBuilder);
            }
            catch
            {
                throw;
            }
        }

        public void UpdateTableData(string updateStatement, IQueryBuilder queryBuilder)
        {
            try
            {
                ExcuteQuery(updateStatement, queryBuilder);
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Excutes sql Statment.
        /// </summary>
        /// <param name="sqlStatment">Sql statment</param>
        /// <param name="queryBuilder">queryBuilder to recieve the condition parameters.</param>
        private void ExcuteQuery(string sqlStatment, IQueryBuilder queryBuilder)
        {
            using (SqlConnection sqlConnection = CreateConnection())
            {
                using (SqlCommand sqlCommand = new(sqlStatment, sqlConnection))
                {
                    List<SqlParameter> conditionalParameters = CloneConditionalParameters(queryBuilder.GetParameters());
                    if (conditionalParameters != null)
                        sqlCommand.Parameters.AddRange(conditionalParameters.ToArray());

                    sqlCommand.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Colones the SqlParameters.
        /// </summary>
        /// <param name="conditionalParameters">Pondition Parameters.</param>
        /// <returns>List of cloned Sql Parameters.</returns>
        private List<SqlParameter> CloneConditionalParameters(IEnumerable<SqlParameter> conditionalParameters)
        {
            if (conditionalParameters == null) return new List<SqlParameter>();

            return conditionalParameters.Select(p => new SqlParameter(p.ParameterName, p.Value)).ToList();
        }

        /// <summary>
        /// Opens SqlConnection.
        /// </summary>
        /// <returns>Open Sql connection.</returns>
        private SqlConnection CreateConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(_connectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
        #endregion
    }
}