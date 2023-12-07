using Business.DynamicModelReflector.Models;

namespace Business.DynamicModelReflector.Interfaces
{
    public interface IDataOperationHelper
    {
        /// <summary>
        /// Retrieves the database information of all the primary keys in a table.
        /// </summary>
        /// <param name="tableName">Database table name.</param>
        /// <returns>List of table data.</returns>
        List<PrimaryKeyInfo> GetPrimaryKeyInfo(string tableName);

        /// <summary>
        /// Retrieves the last Id of a table and increments it with one.
        /// </summary>
        /// <param name="tableName">Database Table name.</param>
        /// <returns>Id incremented by one.</returns>
        int GetNextIdForInsert(string tableName);
    }
}