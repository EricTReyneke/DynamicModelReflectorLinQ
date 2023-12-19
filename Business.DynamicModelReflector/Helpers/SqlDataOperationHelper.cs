using Business.DynamicModelReflector.Interfaces;
using Business.DynamicModelReflector.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Principal;

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
            try
            {
                List<PrimaryKeyInfo> primaryKeyInfoList = new List<PrimaryKeyInfo>();

                string query = @"
            SELECT 
                t.name AS TableName,
                c.name AS ColumnName,
                ty.name AS DataType,
                c.is_identity AS IsIdentity,
                idc.last_value AS LastPrimaryKeyValue
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
            LEFT JOIN 
                sys.identity_columns idc ON c.object_id = idc.object_id AND c.column_id = idc.column_id
            WHERE 
                i.is_primary_key = 1
                AND t.name = @TableName";

                using (SqlConnection connection = CreateConnection())
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@TableName", tableName);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool isIdentity = (bool)reader["IsIdentity"];

                            object lastPrimaryKeyValue = null;

                            if (isIdentity)
                                lastPrimaryKeyValue = reader["LastPrimaryKeyValue"] != DBNull.Value ? int.Parse(reader["LastPrimaryKeyValue"].ToString()) : 1;

                            primaryKeyInfoList.Add(new PrimaryKeyInfo
                            {
                                TableName = reader["TableName"].ToString(),
                                ColumnName = reader["ColumnName"].ToString(),
                                DataType = reader["DataType"].ToString(),
                                IsIdentity = isIdentity,
                                IsGuid = IsGuid(reader["DataType"].ToString()),
                                InsertedValue = isIdentity ? lastPrimaryKeyValue : null,
                            });
                        }
                    }
                }

                return primaryKeyInfoList;
            }
            catch
            {
                throw;
            }
        }

        public int GenerateNextId(string tableName)
        {
            try
            {
                using (SqlConnection connection = CreateConnection())
                {
                    using (SqlCommand command = new($"SELECT MAX(Id) FROM {tableName}", connection))
                    {
                        object result = command.ExecuteScalar();
                        return (result != DBNull.Value && result != null) ? Convert.ToInt32(result) + 1 : 1;
                    }
                }
            }
            catch
            {
                throw;
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
        /// Validates if the datatype from the database is a type of Guid.
        /// </summary>
        /// <param name="dataType">Database datatype.</param>
        /// <returns>True if Guid, false if the datatype is not a Guid.</returns>
        private bool IsGuid(string dataType)
        {
            HashSet<string> integerDataTypes = new() { "uniqueidentifier" };
            return integerDataTypes.Contains(dataType.ToLower());
        }
        #endregion
    }
}