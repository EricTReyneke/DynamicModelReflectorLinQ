using Business.DynamicModelReflector.Interfaces;
using Business.DynamicModelReflector.Models;
using Microsoft.Extensions.Configuration;
using System.Configuration;
using System.Data.SqlClient;

namespace Business.DynamicModelReflector.Helpers
{
    public class SqlDataOperationHelper : IDataOperationHelper
    {
        #region Fields
        /// <summary>
        /// Connection string.
        /// </summary>
        string _connectionString;
        #endregion

        #region Constructors
        /// <summary>
        /// Constructs the SqlDataOperationHelper and sets the connection string from the App.Config.
        /// </summary>
        public SqlDataOperationHelper()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MyDBConnectionString"].ConnectionString;
        }

        /// <summary>
        /// Constructs the SqlDataOperationHelper and allows for IConfiguration injection to retrieve the Connection String From 
        /// The "DBConnectionString" key in the appsettings.json.
        /// </summary>
        /// <param name="configuration">Appsettings from the running program.</param>
        public SqlDataOperationHelper(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DBConnection");
        }
        #endregion

        #region Public Methods
        public List<PrimaryKeyInfo> RetrievePrimaryKeyInfo(string tableName)
        {
            List<PrimaryKeyInfo> primaryKeyInfoList = new();

            string query = @"
            SELECT 
                t.name AS TableName,
                c.name AS ColumnName,
                ty.name AS DataType,
                c.is_identity AS IsIdentity
            FROM 
                sys.tables t
            INNER JOIN 
                sys.indexes i ON t.object_id = i.object_id
            INNER JOIN 
                sys.index_columns ic ON i.object_id = ic.object_id AND i.index_id = ic.index_id
            INNER JOIN 
                sys.columns c ON ic.object_id = c.object_id AND c.column_id = ic.column_id
            INNER JOIN 
                sys.types ty ON c.user_type_id = ty.user_type_id
            WHERE 
                i.is_primary_key = 1
                AND t.name = @TableName";

            using (SqlConnection connection = CreateConnection())
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@TableName", tableName);

                using (SqlDataReader reader = command.ExecuteReader())
                    while (reader.Read())
                        primaryKeyInfoList.Add(new PrimaryKeyInfo
                        {
                            TableName = reader["TableName"].ToString(),
                            ColumnName = reader["ColumnName"].ToString(),
                            DataType = IsIntegerType(reader["DataType"].ToString()),
                            IsIdentity = (bool)reader["IsIdentity"]
                        });
            }

            return primaryKeyInfoList;
        }

        public int GenerateNextId(string tableName)
        {
            using (SqlConnection connection = CreateConnection())
            {
                using (SqlCommand command = new($"SELECT MAX(Id) FROM {tableName}", connection))
                {
                    try
                    {
                        object result = command.ExecuteScalar();
                        return (result != DBNull.Value && result != null) ? Convert.ToInt32(result) + 1 : 1;
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Opens SqlConnection.
        /// </summary>
        /// <returns>Open Sql connection.</returns>
        private SqlConnection CreateConnection()
        {
            SqlConnection sqlConnection = new(_connectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
        
        /// <summary>
        /// Validates if the datatype from the database is a type of int datatype.
        /// </summary>
        /// <param name="dataType">Database datatype.</param>
        /// <returns>True if int, false if the datatype is not a int.</returns>
        private bool IsIntegerType(string dataType)
        {
            HashSet<string> integerDataTypes = new() { "int", "smallint", "tinyint", "bigint" };
            return integerDataTypes.Contains(dataType.ToLower());
        }
        #endregion
    }
}